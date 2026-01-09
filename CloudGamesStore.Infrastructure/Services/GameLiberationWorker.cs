using CloudGamesStore.Application.DTOs;
using CloudGamesStore.Domain.Entities;
using CloudGamesStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CloudGamesStore.Infrastructure.Services
{
    public class GameLiberationWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private IConnection? _connection;
        private IChannel? _channel;

        public GameLiberationWorker(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = _configuration["RabbitMq:Host"] };
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(queue: "fila-liberar-jogo",
                                                     durable: false,
                                                     exclusive: false,
                                                     autoDelete: false,
                                                     arguments: null,
                                                     cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var evento = JsonSerializer.Deserialize<CompraRealizadaEvent>(message);
                    await LiberarJogoParaUsuario(evento);
                }
                catch (Exception ex)
                {
                    throw new Exception($"[GameLiberationWorker] Erro ao processar mensagem: {ex.Message}");
                }
            };

            await _channel.BasicConsumeAsync(queue: "fila-liberar-jogo",
                                             autoAck: true,
                                             consumer: consumer);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null) await _channel.CloseAsync(cancellationToken);
            if (_connection != null) await _connection.CloseAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        private async Task LiberarJogoParaUsuario(CompraRealizadaEvent? evento)
        {
            if (evento == null)
            {
                Console.WriteLine("[Erro] Evento nulo recebido.");
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreCheckoutDbContext>();

                bool jaPossui = await dbContext.GameLibraries.AnyAsync(ug => ug.UserId == evento.UserId && ug.GameId == evento.GameId);

                if (!jaPossui)
                {
                    var novoVinculo = new GameLibrary
                    {
                        UserId = evento.UserId, 
                        GameId = evento.GameId,
                        AcquiredDate = DateTime.Now
                    };

                    dbContext.GameLibraries.Add(novoVinculo);
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine($"[Sucesso] Jogo {evento.GameId} vinculado ao User {evento.UserId}");
                }
            }
        }
    }
}
