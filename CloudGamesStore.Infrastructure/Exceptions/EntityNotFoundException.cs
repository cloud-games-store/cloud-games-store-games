using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGamesStore.Infrastructure.Exceptions
{
    public class EntityNotFoundException : RepositoryException
    {
        public EntityNotFoundException(string entityType, int id)
            : base(entityType, "GetById", $"{entityType} with ID {id} was not found")
        {
        }
    }
}
