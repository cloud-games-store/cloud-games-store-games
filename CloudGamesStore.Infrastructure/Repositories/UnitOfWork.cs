using CloudGamesStore.Domain.Interfaces;
using CloudGamesStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CloudGamesStore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameStoreCheckoutDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _transaction;

        //private IGameRepository? _games;
        private IGameRepository? _games;
        

        public UnitOfWork(
            GameStoreCheckoutDbContext context,
            ILogger<UnitOfWork> logger,
            ILogger<GameRepository> gameLogger)
        {
            _context = context;
            _logger = logger;

            // Initialize repositories with their specific loggers
            //_games = new GameRepository(_context, gameLogger);
            _games = new GameRepository(_context, gameLogger);
        }

        //public IGameRepository Games => _games ??= new GameRepository(_context,
        //    _context.GetService<ILogger<GameRepository>>());

        public IGameRepository Games => _games ??= new GameRepository(_context,
            _context.GetService<ILogger<GameRepository>>());

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes in Unit of Work");
                throw;
            }
        }

        public async Task BeginTransactionAsync()
        {
            try
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                _logger.LogInformation("Transaction started");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error beginning transaction");
                throw;
            }
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    _logger.LogInformation("Transaction committed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error committing transaction");
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    _logger.LogInformation("Transaction rolled back");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back transaction");
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
