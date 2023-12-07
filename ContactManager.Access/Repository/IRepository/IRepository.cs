using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContactManager.Access.Repository.IRepository
{
    /// <summary>
    /// Generic repository interface for data access.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Adds an entity to the table asynchronously.
        /// </summary>
        /// <param name="obj">The entity to add to the table.</param>
        /// <returns>An asynchronous task representing the process of adding the entity.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the write operation was canceled.</exception>
        Task Add(T obj);

        /// <summary>
        /// Updates an entity in the table.
        /// </summary>
        /// <param name="obj">The entity to update in the table.</param>
        void Update(T obj);

        /// <summary>
        /// Retrieves an entity based on the specified filter criteria.
        /// </summary>
        /// <param name="filter">The filter criteria to apply.</param>
        /// <param name="includeProperties">Comma-separated list of related properties to include in the query.</param>
        /// <returns>The entity that matches the filter criteria, or null if no match is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the filter is null.</exception>
        /// <exception cref="FormatException">Thrown if includeProperties is not in the correct format.</exception>
        /// <exception cref="ArgumentException">Thrown if includeProperties contains invalid properties.</exception>
        /// <exception cref="OperationCanceledException">Thrown when there an operation is canceled.</exception>
        Task<T?> Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

        /// <summary>
        /// Retrieves all entities in a table.
        /// </summary>
        /// <param name="includeProperties">Comma-separated list of related properties to include in the query.</param>
        /// <returns>A list of the entities in the table.</returns>
        /// <exception cref="ArgumentException">Thrown if includeProperties includes invalid properties.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the property is null or query result is empty.</exception>
        /// <exception cref="OperationCanceledException">Thrown when the conversion to list is canceled.</exception>
        Task<IEnumerable<T>> GetAll(string? includeProperties = null);

        /// <summary>
        /// Removes a single entity from a table.
        /// </summary>
        /// <param name="obj">The entity to remove from the table.</param>
        void Remove(T obj);

        /// <summary>
        /// Removes a range of entities from a table.
        /// </summary>
        /// <param name="objs">The range of entities to remove from the table.</param>
        void RemoveRange(IEnumerable<T> objs);
    }
}
