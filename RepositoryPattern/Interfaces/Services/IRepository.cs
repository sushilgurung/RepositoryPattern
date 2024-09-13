using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Gurung.RepositoryPattern.Interfaces.Services
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Queryable { get; }
        IQueryable<T> GetQueryable();
        Expression<Func<T, bool>> GeneratePredicate(Expression<Func<T, bool>> predicate);

        #region Get
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllAsNoTracking();

        IEnumerable<T> GetAll(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        IEnumerable<T> GetAllAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsNoTrackingAsync();

        Task<IEnumerable<T>> GetAllAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<IEnumerable<T>> GetAllAsNoTrackingAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);

        IEnumerable<T> GetAll(int pageNumber, int pageSize);
        IEnumerable<T> GetAllAsNoTracking(int pageNumber, int pageSize);

        IEnumerable<T> GetAll(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        IEnumerable<T> GetAllAsNoTracking(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);

        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);
        Task<IEnumerable<T>> GetAllAsNoTrackingAsync(int pageNumber, int pageSize);

        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<IEnumerable<T>> GetAllAsNoTrackingAsync(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);


        #endregion

        #region GetById
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        T GetByIdAsNoTracking(int id);
        Task<T> GetByIdAsNoTrackingAsync(int id);

        T GetById(long id);
        Task<T> GetByIdAsync(long id);
        T GetByIdAsNoTracking(long id);
        Task<T> GetByIdAsNoTrackingAsync(long id);


        T GetById(string id);
        Task<T> GetByIdAsync(string id);
        T GetByIdAsNoTracking(string id);
        Task<T> GetByIdAsNoTrackingAsync(string id);

        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        T GetByIdAsNoTracking(Guid id);
        Task<T> GetByIdAsNoTrackingAsync(Guid id);

        #endregion

        #region First or Default
        T FirstOrDefault();
        Task<T> FirstOrDefaultAsync();
        T FirstOrDefaultAsNoTracking();
        Task<T> FirstOrDefaultAsyncAsNoTracking();


        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        T FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsyncAsNoTracking(Expression<Func<T, bool>> predicate);


        T FirstOrDefault(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<T> FirstOrDefaultAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        T FirstOrDefaultAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<T> FirstOrDefaultAsyncAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);


        T FirstOrDefault(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        T FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<T> FirstOrDefaultAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        #endregion

        #region find

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);

        IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);

        Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);

        #endregion

        #region Add
        T Add(T entity);
        /// <summary>
        /// Asynchronously adds a new entity to the database context and returns the added entity.
        /// </summary>
        /// <param name="entity">
        /// The entity to add to the database context. This entity is tracked by the context and will be inserted into the database when `SaveChanges` is called.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token to observe while waiting for the asynchronous operation to complete. The default value is <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the added entity, which will be in the Added state in the context until changes are saved.
        /// </returns>
        /// <example>
        /// <code>
        /// // Create a new entity
        /// var newEntity = new MyEntity { Name = "New Entity", Value = 123 };
        ///
        /// // Asynchronously add the new entity to the context
        /// var addedEntity = await _repository.AddAsync(newEntity);
        ///
        /// // Save changes to persist the new entity in the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the operation is canceled by the <paramref name="cancellationToken"/>.
        /// </exception>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        IEnumerable<T> AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        #endregion

        #region Update
        T Update(T entity);
        Task<T> UpdateAsync(T entity);
        IEnumerable<T> UpdateRange(List<T> entities);
        Task<IEnumerable<T>> UpdateRangeAsync(List<T> entities);
        #endregion

        #region Delete
        void Remove(T entity);
        Task RemoveAsync(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        #endregion


        #region Count
        int Count();
        int Count(Expression<Func<T, bool>> where);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> where);


        long CountLong();
        long CountLong(Expression<Func<T, bool>> where);
        Task<long> CountLongAsync();
        Task<long> CountLongAsync(Expression<Func<T, bool>> where);


        #endregion

        #region Any
        bool Any();
        bool Any(Expression<Func<T, bool>> where);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> where);
        #endregion

        #region Save Changes
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        #endregion

        #region Transaction Management
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        #endregion

  


    }
}
