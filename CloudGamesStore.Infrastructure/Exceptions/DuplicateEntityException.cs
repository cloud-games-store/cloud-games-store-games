namespace CloudGamesStore.Infrastructure.Exceptions
{
    public class DuplicateEntityException : RepositoryException
    {
        public DuplicateEntityException(string entityType, string field, string value)
            : base(entityType, "Create", $"{entityType} with {field} '{value}' already exists")
        {
        }
    }
}
