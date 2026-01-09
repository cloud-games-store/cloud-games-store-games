namespace CloudGamesStore.Application.DTOs
{
    public record CompraRealizadaEvent(
        Guid TransactionId,
        Guid UserId,
        int GameId,
        bool Success,
        DateTime? AcquiredDate
    );
}
