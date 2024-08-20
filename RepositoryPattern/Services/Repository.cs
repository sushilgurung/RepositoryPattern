using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RepositoryPattern.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryPattern.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly DbContext _dbContext;
        private IDbContextTransaction _transaction;
        public Repository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IQueryable<T> Queryable
        {
            get
            {
                return GetQueryable();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetQueryable()
        {
            return _dbContext.Set<T>();
        }

        #region Get All
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAllAsNoTracking()
        {
            return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }
        #endregion

        #region Get by Id
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(long id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion


        #region FirstorDefault

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T FirstOrDefault()
        {
            return _dbContext.Set<T>().FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync()
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Retrieves the first or default record, applying sorting criteria based on the provided order-by specifications.
        /// </summary>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// The first entity that matches the criteria after applying the sorting, or null if no entities are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define sorting criteria directly as parameters
        /// var country =  _countryRepository.FirstOrDefault(
        ///     (x => x.Name, false),       // Sort by Name ascending
        ///     (x => x.Population, true),   // Then by Population descending
        ///     (x => x.Area, false)         // Finally by Area ascending
        /// );
        /// </code>
        /// </example>
        public T FirstOrDefault(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            if (orderBys == null || !orderBys.Any())
                throw new ArgumentException("Order by selectors cannot be null or empty", nameof(orderBys));

            IQueryable<T> query = _dbContext.Set<T>();

            // Apply the first sorting criterion
            IOrderedQueryable<T> orderedQuery = orderBys[0].Descending
                ? query.OrderByDescending(orderBys[0].KeySelector)
                : query.OrderBy(orderBys[0].KeySelector);

            // Apply subsequent sorting criteria using ThenBy or ThenByDescending
            for (int i = 1; i < orderBys.Length; i++)
            {
                // Additional sorting orders
                var orderBy = orderBys[i];
                orderedQuery = orderBy.Descending
                    ? orderedQuery.ThenByDescending(orderBy.KeySelector)
                    : orderedQuery.ThenBy(orderBy.KeySelector);
            }
            return orderedQuery.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the first or default record, applying sorting criteria based on the provided order-by specifications.
        /// </summary>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// The first entity that matches the criteria after applying the sorting, or null if no entities are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define sorting criteria directly as parameters
        /// var country = await _countryRepository.FirstOrDefaultAsync(
        ///     (x => x.Name, false),       // Sort by Name ascending
        ///     (x => x.Population, true),   // Then by Population descending
        ///     (x => x.Area, false)         // Finally by Area ascending
        /// );
        /// </code>
        /// </example>
        public async Task<T> FirstOrDefaultAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            if (orderBys == null || !orderBys.Any())
                throw new ArgumentException("Order by selectors cannot be null or empty", nameof(orderBys));

            IQueryable<T> query = _dbContext.Set<T>();

            // Apply the first sorting criterion
            IOrderedQueryable<T> orderedQuery = orderBys[0].Descending
                ? query.OrderByDescending(orderBys[0].KeySelector)
                : query.OrderBy(orderBys[0].KeySelector);

            // Apply subsequent sorting criteria using ThenBy or ThenByDescending
            for (int i = 1; i < orderBys.Length; i++)
            {
                // Additional sorting orders
                var orderBy = orderBys[i];
                orderedQuery = orderBy.Descending
                    ? orderedQuery.ThenByDescending(orderBy.KeySelector)
                    : orderedQuery.ThenBy(orderBy.KeySelector);
            }

            return await orderedQuery.FirstOrDefaultAsync();
        }


        public T FirstOrDefault(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (orderBys == null || !orderBys.Any())
                throw new ArgumentException("Order by selectors cannot be null or empty", nameof(orderBys));

            IQueryable<T> query = _dbContext.Set<T>().Where(predicate);

            // Apply the first sorting criterion
            IOrderedQueryable<T> orderedQuery = orderBys[0].Descending
                ? query.OrderByDescending(orderBys[0].KeySelector)
                : query.OrderBy(orderBys[0].KeySelector);

            // Apply subsequent sorting criteria using ThenBy or ThenByDescending
            for (int i = 1; i < orderBys.Length; i++)
            {
                // Additional sorting orders
                var orderBy = orderBys[i];
                orderedQuery = orderBy.Descending
                    ? orderedQuery.ThenByDescending(orderBy.KeySelector)
                    : orderedQuery.ThenBy(orderBy.KeySelector);
            }

            return orderedQuery.FirstOrDefault();
        }



        /// <summary>
        /// Retrieves the first or default record based on the provided filtering predicate and sorting criteria.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for filtering the records. The method will return the first entity that matches this criteria.
        /// For example, (x => x.IsActive) filters the records to include only those where the IsActive property is true.
        /// </param>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// The first entity that matches the filtering criteria after applying the sorting, or null if no entities match the criteria.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the filtering predicate is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define a filtering predicate
        /// Expression<Func<Country, bool>> filter = x => x.IsActive && x.Population > 1000000;
        ///
        /// // Call the FirstOrDefaultAsync method with filtering and sorting criteria directly
        /// var country = await _countryRepository.FirstOrDefaultAsync(
        ///     filter,
        ///     (x => x.Name, false),       // Sort by Name ascending
        ///     (x => x.Population, true),   // Then by Population descending
        ///     (x => x.Area, false)         // Finally by Area ascending
        /// );
        /// </code>
        /// </example>
        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (orderBys == null || !orderBys.Any())
                throw new ArgumentException("Order by selectors cannot be null or empty", nameof(orderBys));

            IQueryable<T> query = _dbContext.Set<T>().Where(predicate);

            // Apply the first sorting criterion
            IOrderedQueryable<T> orderedQuery = orderBys[0].Descending
                ? query.OrderByDescending(orderBys[0].KeySelector)
                : query.OrderBy(orderBys[0].KeySelector);

            // Apply subsequent sorting criteria using ThenBy or ThenByDescending
            for (int i = 1; i < orderBys.Length; i++)
            {
                // Additional sorting orders
                var orderBy = orderBys[i];
                orderedQuery = orderBy.Descending
                    ? orderedQuery.ThenByDescending(orderBy.KeySelector)
                    : orderedQuery.ThenBy(orderBy.KeySelector);
            }

            return await orderedQuery.FirstOrDefaultAsync();
        }



        #endregion

        #region Add
        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
            return entities;
        }


        #endregion

        #region Update
        public T Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return await Task.FromResult(entity);
        }

        public IEnumerable<T> UpdateRange(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> UpdateRangeAsync(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return await Task.FromResult<IEnumerable<T>>(entities);
        }

        #endregion


        #region SaveChanges
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion



        #region Transaction Management

        /// <summary>
        /// Begins a new transaction asynchronously.
        /// </summary>
        /// <returns>The transaction object.</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
            return _transaction;
        }

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _transaction?.Commit();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        #endregion


    }
}
