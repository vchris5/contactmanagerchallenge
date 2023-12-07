using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ContactManager.Access.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Access.Repository
{
    // Generic repository implementation for data access.
    public class Repository<T> : IRepository<T> where T : class
    {
        // Internal DbSet for the specified entity type.
        internal DbSet<T> dbSet;

        // Constructor that initializes the internal DbSet.
        public Repository(DbSet<T> _dbSet)
        {
            dbSet = _dbSet;
        }

        // Adds a new entity to the database asynchronously.
        public async Task Add(T obj)
        {
            await dbSet.AddAsync(obj);
        }

        // Retrieves a single entity based on the specified filter criteria.
        public async Task<T?> Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet.Where(filter);

            // Include related properties if specified.
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        // Retrieves all entities in the table.
        public async Task<IEnumerable<T>> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            // Include related properties if specified.
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }

        // Removes a single entity from the table.
        public void Remove(T obj)
        {
            dbSet.Remove(obj);
        }

        // Removes a range of entities from the table.
        public void RemoveRange(IEnumerable<T> objs)
        {
            dbSet.RemoveRange(objs);
        }

        // Updates an existing entity in the table.
        public void Update(T obj)
        {
            dbSet.Update(obj);
        }
    }
}
