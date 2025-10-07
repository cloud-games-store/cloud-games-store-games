namespace CloudGamesStore.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IGameRepository Games { get; }
        IGameRepository Games { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
