using CloudGamesStore.Domain.Interfaces;
using CloudGamesStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudGamesStore.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly GameStoreCheckoutDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<BaseRepository<T>> _logger;

        protected BaseRepository(GameStoreCheckoutDbContext context,
            ILogger<BaseRepository<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity with ID {Id}", id);
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all entities of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Entity of type {EntityType} created successfully", typeof(T).Name);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entity of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Entity of type {EntityType} updated successfully", typeof(T).Name);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Entity of type {EntityType} with ID {Id} deleted successfully", typeof(T).Name, id);
                }
                else
                {
                    _logger.LogWarning("Entity of type {EntityType} with ID {Id} not found for deletion", typeof(T).Name, id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity of type {EntityType} with ID {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id) != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if entity with ID {Id} exists", id);
                throw;
            }
        }

        public virtual async Task<int> CountAsync()
        {
            try
            {
                return await _dbSet.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting entities of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

    }
}
