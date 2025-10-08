namespace CloudGamesStore.Infrastructure.Exceptions
{
    public class RepositoryException : Exception
    {
        public string EntityType { get; }
        public string Operation { get; }

        public RepositoryException(string entityType, string operation, string message)
            : base(message)
        {
            EntityType = entityType;
            Operation = operation;
        }

        public RepositoryException(string entityType, string operation, string message, Exception innerException)
            : base(message, innerException)
        {
            EntityType = entityType;
            Operation = operation;
        }
    }
}
