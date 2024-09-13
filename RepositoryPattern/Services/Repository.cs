using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Gurung.RepositoryPattern.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Gurung.RepositoryPattern.Helpers;

namespace Gurung.RepositoryPattern.Services
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
        /// Provides an <see cref="IQueryable{T}"/> instance for the current entity type, 
        /// allowing for LINQ queries to be composed and executed against the underlying data source.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> that can be used to query the underlying data set 
        /// of the current entity type.
        /// </returns>
        /// <remarks>
        /// This property is typically used to build queries that can be further filtered, sorted, 
        /// and projected before being executed. It returns the base queryable object, allowing 
        /// for custom query logic to be added before retrieving the data.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve all entities of type T where the Name property starts with "A"
        /// var query = _repository.Queryable.Where(x => x.Name.StartsWith("A"));
        /// var results = query.ToList();
        /// </code>
        /// </example>
        public virtual IQueryable<T> Queryable
        {
            get
            {
                return GetQueryable();
            }
        }

        /// <summary>
        /// Retrieves an <see cref="IQueryable{T}"/> for the specified entity type <typeparamref name="T"/>, 
        /// allowing for LINQ queries to be composed and executed against the underlying data source.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> representing the data set for the specified entity type <typeparamref name="T"/>.
        /// </returns>
        /// <remarks>
        /// This method accesses the underlying database context to provide a queryable collection of entities.
        /// It is typically used as a starting point for building LINQ queries that can filter, sort, and project data
        /// before executing the query.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve all entities of type T where the IsActive property is true
        /// var queryable = _repository.GetQueryable().Where(x => x.IsActive);
        /// var activeEntities = queryable.ToList();
        /// </code>
        /// </example>
        public IQueryable<T> GetQueryable()
        {
            return _dbContext.Set<T>();
        }



        #region Get All
        /// <summary>
        /// Retrieves all entities of the specified type <typeparamref name="T"/> from the data source.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing all entities of type <typeparamref name="T"/> stored in the database.
        /// </returns>
        /// <remarks>
        /// This method queries the entire data set for the specified entity type and returns it as a list.
        /// It is typically used when all records of the entity are required to be fetched at once.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve all entities of type T from the database
        /// var allEntities = _repository.GetAll();
        /// foreach (var entity in allEntities)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>
        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        /// <summary>
        /// Retrieves all entities of the specified type <typeparamref name="T"/> from the data source without tracking them in the context.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing all entities of type <typeparamref name="T"/> retrieved from the database without change tracking.
        /// </returns>
        /// <remarks>
        /// This method queries the entire data set for the specified entity type and returns it as a list without enabling Entity Framework's change tracking.
        /// It is useful for read-only operations where the entities do not need to be tracked for updates.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve all entities of type T from the database without tracking
        /// var allEntitiesNoTracking = _repository.GetAllAsNoTracking();
        /// foreach (var entity in allEntitiesNoTracking)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>
        public IEnumerable<T> GetAllAsNoTracking()
        {
            return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        public IEnumerable<T> GetAll(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.FilterAndOrderBy(null, orderBys).ToList();
        }

        public IEnumerable<T> GetAllAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.FilterAndOrderBy(null, orderBys).AsNoTracking().ToList();
        }

        /// <summary>
        /// Asynchronously retrieves all entities of the specified type <typeparamref name="T"/> from the data source.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> 
        /// with all entities of type <typeparamref name="T"/> stored in the database.
        /// </returns>
        /// <remarks>
        /// This method asynchronously queries the entire data set for the specified entity type and returns it as a list. 
        /// It is useful in scenarios where non-blocking operations are preferred.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously retrieve all entities of type T from the database
        /// var allEntities = await _repository.GetAllAsync();
        /// foreach (var entity in allEntities)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>

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

        public async Task<IEnumerable<T>> GetAllAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FilterAndOrderBy(null, orderBys).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FilterAndOrderBy(null, orderBys).AsNoTracking().ToListAsync();
        }
    
        //
        public IEnumerable<T> GetAll(int pageNumber, int pageSize)
        {
            return _dbContext.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<T> GetAllAsNoTracking(int pageNumber, int pageSize)
        {
            return _dbContext.Set<T>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }


        public IEnumerable<T> GetAll(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.FilterAndOrderBy(null, orderBys).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<T> GetAllAsNoTracking(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.FilterAndOrderBy(null, orderBys).AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync(int pageNumber, int pageSize)
        {
            return await _dbContext.Set<T>().AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FilterAndOrderBy(null, orderBys).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FilterAndOrderBy(null, orderBys).AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        #endregion

        #region Get by Id
        /// <summary>
        /// Retrieves an entity of the specified type <typeparamref name="T"/> by its primary key.
        /// </summary>
        /// <param name="id">
        /// The primary key value of the entity to be retrieved.
        /// </param>
        /// <returns>
        /// The entity of type <typeparamref name="T"/> with the specified primary key value, or null if no entity is found.
        /// </returns>
        /// <remarks>
        /// This method queries the database for an entity with the given primary key and returns it. 
        /// If no entity with the specified key is found, the method returns null.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve an entity of type T with a specific ID
        /// var entity = _repository.GetById(1);
        /// if (entity != null)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        /// <summary>
        /// Retrieves an entity of the specified type <typeparamref name="T"/> by its primary key.
        /// </summary>
        /// <param name="id">
        /// The primary key value of the entity to be retrieved as a <see cref="long"/>.
        /// </param>
        /// <returns>
        /// The entity of type <typeparamref name="T"/> with the specified primary key value, or null if no entity is found.
        /// </returns>
        /// <remarks>
        /// This method queries the database for an entity with the given primary key of type <see cref="long"/> and returns it. 
        /// If no entity with the specified key is found, the method returns null.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve an entity of type T with a specific long ID
        /// var entity = _repository.GetById(1000000001L);
        /// if (entity != null)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>

        public T GetById(long id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        /// <summary>
        /// Asynchronously retrieves an entity of the specified type <typeparamref name="T"/> by its primary key.
        /// </summary>
        /// <param name="id">
        /// The primary key value of the entity to be retrieved as an <see cref="int"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the entity of type <typeparamref name="T"/> 
        /// with the specified primary key value, or null if no entity is found.
        /// </returns>
        /// <remarks>
        /// This method asynchronously queries the database for an entity with the given primary key of type <see cref="int"/> 
        /// and returns it. If no entity with the specified key is found, the method returns null.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously retrieve an entity of type T with a specific ID
        /// var entity = await _repository.GetByIdAsync(1);
        /// if (entity != null)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Asynchronously retrieves an entity of the specified type <typeparamref name="T"/> by its primary key.
        /// </summary>
        /// <param name="id">
        /// The primary key value of the entity to be retrieved, specified as a <see cref="long"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the entity of type <typeparamref name="T"/> 
        /// with the specified primary key value, or null if no entity is found.
        /// </returns>
        /// <remarks>
        /// This method performs an asynchronous query on the database to find an entity with the given primary key of type <see cref="long"/>. 
        /// If no entity with the specified key exists, the method returns null.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously retrieve an entity of type T with a specific long ID
        /// var entity = await _repository.GetByIdAsync(10000000001L);
        /// if (entity != null)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>

        public async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        #endregion

        #region FirstorDefault

        /// <summary>
        /// Retrieves the first entity of the specified type <typeparamref name="T"/> from the data source, or a default value if no entities are found.
        /// </summary>
        /// <returns>
        /// The first entity of type <typeparamref name="T"/> from the data source, or the default value for the type if no entities are present.
        /// </returns>
        /// <remarks>
        /// This method queries the database for the first entity of the specified type. If there are no entities in the data source, 
        /// it returns the default value for the type <typeparamref name="T"/> (e.g., null for reference types). This method does not 
        /// support ordering; it simply returns the first entity it encounters.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve the first entity of type T from the database
        /// var firstEntity = _repository.FirstOrDefault();
        /// if (firstEntity != null)
        /// {
        ///     Console.WriteLine(firstEntity.Name);
        /// }
        /// </code>
        /// </example>
        public T FirstOrDefault()
        {
            return _dbContext.Set<T>().FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously retrieves the first entity of the specified type <typeparamref name="T"/> from the data source, 
        /// or a default value if no entities are found.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first entity of type <typeparamref name="T"/> 
        /// from the data source, or the default value for the type if no entities are present.
        /// </returns>
        /// <remarks>
        /// This method performs an asynchronous query to retrieve the first entity of the specified type. If no entities are found in the data source, 
        /// it returns the default value for the type <typeparamref name="T"/> (e.g., null for reference types). This method does not support ordering; 
        /// it simply returns the first entity it encounters asynchronously.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously retrieve the first entity of type T from the database
        /// var firstEntity = await _repository.FirstOrDefaultAsync();
        /// if (firstEntity != null)
        /// {
        ///     Console.WriteLine(firstEntity.Name);
        /// }
        /// </code>
        /// </example>
        public async Task<T> FirstOrDefaultAsync()
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves the first entity of the specified type <typeparamref name="T"/> from the data source without tracking it in the context,
        /// or a default value if no entities are found.
        /// </summary>
        /// <returns>
        /// The first entity of type <typeparamref name="T"/> from the data source without tracking, 
        /// or the default value for the type if no entities are present.
        /// </returns>
        /// <remarks>
        /// This method queries the database for the first entity of the specified type and returns it without enabling change tracking. 
        /// If no entities are found, it returns the default value for the type <typeparamref name="T"/> (e.g., null for reference types). 
        /// This is useful for read-only operations where tracking is not needed, improving performance in scenarios where entities are not 
        /// updated after retrieval.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve the first entity of type T from the database without tracking
        /// var firstEntityNoTracking = _repository.FirstOrDefaultAsNoTracking();
        /// if (firstEntityNoTracking != null)
        /// {
        ///     Console.WriteLine(firstEntityNoTracking.Name);
        /// }
        /// </code>
        /// </example>
        public T FirstOrDefaultAsNoTracking()
        {
            return _dbContext.Set<T>().AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously retrieves the first entity of the specified type <typeparamref name="T"/> from the data source without tracking it in the context,
        /// or a default value if no entities are found.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first entity of type <typeparamref name="T"/> 
        /// from the data source without tracking, or the default value for the type if no entities are present.
        /// </returns>
        /// <remarks>
        /// This method performs an asynchronous query to retrieve the first entity of the specified type without enabling change tracking. 
        /// If no entities are found, it returns the default value for the type <typeparamref name="T"/> (e.g., null for reference types). 
        /// This is useful for read-only operations where tracking is not required, improving performance by avoiding unnecessary 
        /// change tracking overhead.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously retrieve the first entity of type T from the database without tracking
        /// var firstEntityNoTracking = await _repository.FirstOrDefaultAsyncAsNoTracking();
        /// if (firstEntityNoTracking != null)
        /// {
        ///     Console.WriteLine(firstEntityNoTracking.Name);
        /// }
        /// </code>
        /// </example>
        public async Task<T> FirstOrDefaultAsyncAsNoTracking()
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves the first entity of the specified type <typeparamref name="T"/> from the data source that satisfies the provided predicate,
        /// or a default value if no matching entity is found.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entity. This expression is used to filter the entities in the data source.
        /// </param>
        /// <returns>
        /// The first entity of type <typeparamref name="T"/> that matches the specified predicate, or the default value for the type if no entity is found.
        /// </returns>
        /// <remarks>
        /// This method queries the database for the first entity that matches the criteria defined by the <paramref name="predicate"/>. 
        /// If no entity matches the criteria, it returns the default value for the type <typeparamref name="T"/> (e.g., null for reference types). 
        /// This method does not support ordering and returns the first entity that meets the criteria.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve the first entity of type T where the Name property equals "John"
        /// var entity = _repository.FirstOrDefault(x => x.Name == "John");
        /// if (entity != null)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously retrieves the first entity of the specified type <typeparamref name="T"/> from the data source that satisfies the provided predicate,
        /// or a default value if no matching entity is found.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entity. This expression is used to filter the entities in the data source.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first entity of type <typeparamref name="T"/> 
        /// that matches the specified predicate, or the default value for the type if no entity is found.
        /// </returns>
        /// <remarks>
        /// This method performs an asynchronous query to find the first entity that matches the criteria defined by the <paramref name="predicate"/>. 
        /// If no entity meets the criteria, it returns the default value for the type <typeparamref name="T"/> (e.g., null for reference types). 
        /// This method does not support ordering and returns the first entity that matches the predicate asynchronously.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously retrieve the first entity of type T where the Name property equals "John"
        /// var entity = await _repository.FirstOrDefaultAsync(x => x.Name == "John");
        /// if (entity != null)
        /// {
        ///     Console.WriteLine(entity.Name);
        /// }
        /// </code>
        /// </example>
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves the first entity of the specified type <typeparamref name="T"/> from the data source that satisfies the provided predicate,
        /// without tracking the entity in the context, or a default value if no matching entity is found.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entity. This expression is used to filter the entities in the data source.
        /// </param>
        /// <returns>
        /// The first entity of type <typeparamref name="T"/> that matches the specified predicate without tracking, 
        /// or the default value for the type if no entity is found.
        /// </returns>
        /// <remarks>
        /// This method queries the database for the first entity that matches the criteria defined by the <paramref name="predicate"/> and returns it without enabling change tracking. 
        /// If no entity meets the criteria, it returns the default value for the type <typeparamref name="T"/> (e.g., null for reference types). 
        /// This approach is useful for read-only operations where tracking is not required, potentially improving performance by avoiding the overhead associated with change tracking.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve the first entity of type T where the Name property equals "John" without tracking
        /// var entityNoTracking = _repository.FirstOrDefaultAsNoTracking(x => x.Name == "John");
        /// if (entityNoTracking != null)
        /// {
        ///     Console.WriteLine(entityNoTracking.Name);
        /// }
        /// </code>
        /// </example>
        public T FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously retrieves the first entity of the specified type <typeparamref name="T"/> from the data source that satisfies the provided predicate,
        /// without tracking the entity in the context, or a default value if no matching entity is found.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entity. This expression is used to filter the entities in the data source.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first entity of type <typeparamref name="T"/> 
        /// that matches the specified predicate without tracking, or the default value for the type if no entity is found.
        /// </returns>
        /// <remarks>
        /// This method performs an asynchronous query to find the first entity that matches the criteria defined by the <paramref name="predicate"/> 
        /// and returns it without enabling change tracking. If no entity meets the criteria, it returns the default value for the type <typeparamref name="T"/> 
        /// (e.g., null for reference types). This approach is useful for read-only operations where tracking is not necessary, potentially improving performance 
        /// by avoiding the overhead associated with change tracking.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously retrieve the first entity of type T where the Name property equals "John" without tracking
        /// var entityNoTracking = await _repository.FirstOrDefaultAsyncAsNoTracking(x => x.Name == "John");
        /// if (entityNoTracking != null)
        /// {
        ///     Console.WriteLine(entityNoTracking.Name);
        /// }
        /// </code>
        /// </example>
        public async Task<T> FirstOrDefaultAsyncAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).AsNoTracking().FirstOrDefaultAsync();
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

        /// <summary>
        /// Retrieves the first or default record, applying sorting criteria based on the provided order-by specifications, 
        /// and without tracking the entity in the context.
        /// </summary>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// The first entity that matches the criteria after applying the sorting, and without tracking, or null if no entities are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided, or if <paramref name="orderBys"/> is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define sorting criteria directly as parameters, without tracking
        /// var country = _countryRepository.FirstOrDefaultAsNoTracking(
        ///     (x => x.Name, false),       // Sort by Name ascending
        ///     (x => x.Population, true),   // Then by Population descending
        ///     (x => x.Area, false)         // Finally by Area ascending
        /// );
        /// </code>
        /// </example>
        public T FirstOrDefaultAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
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
            return orderedQuery.AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously retrieves the first or default record, applying sorting criteria based on the provided order-by specifications,
        /// and without tracking the entity in the context.
        /// </summary>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first entity that matches the criteria 
        /// after applying the sorting, and without tracking, or null if no entities are found.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided, or if <paramref name="orderBys"/> is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Asynchronously define sorting criteria directly as parameters, without tracking
        /// var country = await _countryRepository.FirstOrDefaultAsyncAsNoTracking(
        ///     (x => x.Name, false),       // Sort by Name ascending
        ///     (x => x.Population, true),   // Then by Population descending
        ///     (x => x.Area, false)         // Finally by Area ascending
        /// );
        /// if (country != null)
        /// {
        ///     Console.WriteLine($"{country.Name}, {country.Population}, {country.Area}");
        /// }
        /// </code>
        /// </example>
        public async Task<T> FirstOrDefaultAsyncAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
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
            return await orderedQuery.AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves the first or default record that matches the specified predicate, applying sorting criteria based on the provided order-by specifications.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entity. This expression is used to filter the entities before applying sorting.
        /// </param>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// The first entity that matches the predicate after applying the sorting criteria, or null if no entities are found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided, or if <paramref name="orderBys"/> is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define filtering and sorting criteria directly as parameters
        /// var result = _repository.FirstOrDefault(
        ///     x => x.IsActive,             // Filter by IsActive property
        ///     (x => x.Name, false),       // Sort by Name ascending
        ///     (x => x.Population, true)    // Then by Population descending
        /// );
        /// if (result != null)
        /// {
        ///     Console.WriteLine($"{result.Name}, {result.Population}");
        /// }
        /// </code>
        /// </example>
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
        ///     x => x.IsActive,   
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

        /// <summary>
        /// Retrieves the first or default record that matches the specified predicate, applying sorting criteria based on the provided order-by specifications,
        /// and without tracking the entity in the context.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entity. This expression is used to filter the entities before applying sorting.
        /// </param>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// The first entity that matches the predicate after applying the sorting criteria and without tracking, or null if no entities are found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided, or if <paramref name="orderBys"/> is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define filtering and sorting criteria directly as parameters, without tracking
        /// var result = _repository.FirstOrDefaultAsNoTracking(
        ///     x => x.IsActive,             // Filter by IsActive property
        ///     (x => x.Name, false),       // Sort by Name ascending
        ///     (x => x.Population, true)    // Then by Population descending
        /// );
        /// if (result != null)
        /// {
        ///     Console.WriteLine($"{result.Name}, {result.Population}");
        /// }
        /// </code>
        /// </example>
        public T FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (orderBys == null || !orderBys.Any())
                throw new ArgumentException("Order by selectors cannot be null or empty", nameof(orderBys));

            IQueryable<T> query = _dbContext.Set<T>().Where(predicate).AsNoTracking();

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
            return orderedQuery.AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously retrieves the first or default record that matches the specified predicate, applying sorting criteria based on the provided order-by specifications,
        /// and without tracking the entity in the context.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entity. This expression is used to filter the entities before applying sorting.
        /// </param>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first entity that matches the predicate 
        /// after applying the sorting criteria and without tracking, or null if no entities are found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided, or if <paramref name="orderBys"/> is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Asynchronously define filtering and sorting criteria directly as parameters, without tracking
        /// var result = await _repository.FirstOrDefaultAsyncAsNoTracking(
        ///     x => x.IsActive,             // Filter by IsActive property
        ///     (x => x.Name, false),       // Sort by Name ascending
        ///     (x => x.Population, true)    // Then by Population descending
        /// );
        /// if (result != null)
        /// {
        ///     Console.WriteLine($"{result.Name}, {result.Population}");
        /// }
        /// </code>
        /// </example>
        public virtual async Task<T> FirstOrDefaultAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (orderBys == null || !orderBys.Any())
                throw new ArgumentException("Order by selectors cannot be null or empty", nameof(orderBys));

            IQueryable<T> query = _dbContext.Set<T>().Where(predicate).AsNoTracking();

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
            return await orderedQuery.AsNoTracking().FirstOrDefaultAsync();
        }

        #endregion

        #region find
        /// <summary>
        /// Retrieves all records that match the specified predicate.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entities. This expression is used to filter the entities that are returned.
        /// </param>
        /// <returns>
        /// A collection of entities that match the specified predicate. The collection is returned as an enumerable list of entities.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define filtering criteria directly as a parameter
        /// var activeUsers = _repository.Find(x => x.IsActive);
        /// foreach (var user in activeUsers)
        /// {
        ///     Console.WriteLine($"{user.Name} is active.");
        /// }
        /// </code>
        /// </example>
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// Retrieves all records that match the specified predicate, applying sorting criteria based on the provided order-by specifications.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entities. This expression is used to filter the entities that are returned.
        /// </param>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// A collection of entities that match the specified predicate and are sorted according to the provided order-by criteria. 
        /// The collection is returned as an enumerable list of entities.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided or if the <paramref name="orderBys"/> parameter is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define filtering and sorting criteria directly as parameters
        /// var results = _repository.Find(
        ///     x => x.IsActive,               // Filter by IsActive property
        ///     (x => x.Name, false),         // Sort by Name in ascending order
        ///     (x => x.Population, true)     // Then by Population in descending order
        /// );
        /// foreach (var item in results)
        /// {
        ///     Console.WriteLine($"{item.Name}: {item.Population}");
        /// }
        /// </code>
        /// </example>
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
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
            return orderedQuery.ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.FilterAndOrderBy(predicate, orderBys).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }


        /// <summary>
        /// Retrieves all records that match the specified predicate without tracking them in the context.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entities. This expression is used to filter the entities that are returned.
        /// </param>
        /// <returns>
        /// A collection of entities that match the specified predicate and are not tracked by the context. The collection is returned as an enumerable list of entities.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define filtering criteria directly as a parameter, without tracking
        /// var activeUsers = _repository.FindAsNoTracking(x => x.IsActive);
        /// foreach (var user in activeUsers)
        /// {
        ///     Console.WriteLine($"{user.Name} is active.");
        /// }
        /// </code>
        /// </example>
        public IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).AsNoTracking().ToList();
        }

        /// <summary>
        /// Retrieves all records that match the specified predicate, applying sorting criteria based on the provided order-by specifications, without tracking the entities in the context.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entities. This expression is used to filter the entities that are returned.
        /// </param>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// A collection of entities that match the specified predicate and are sorted according to the provided order-by criteria. 
        /// The collection is returned as an enumerable list of entities, and the entities are not tracked by the context.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided or if the <paramref name="orderBys"/> parameter is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Define filtering and sorting criteria directly as parameters, without tracking
        /// var results = _repository.FindAsNoTracking(
        ///     x => x.IsActive,               // Filter by IsActive property
        ///     (x => x.Name, false),         // Sort by Name in ascending order
        ///     (x => x.Population, true)     // Then by Population in descending order
        /// );
        /// foreach (var item in results)
        /// {
        ///     Console.WriteLine($"{item.Name}: {item.Population}");
        /// }
        /// </code>
        /// </example>
        public IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.FilterAndOrderBy(predicate, orderBys).AsNoTracking().ToList();
        }

        public IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return query.FilterAndOrderBy(predicate, orderBys).AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }



        /// <summary>
        /// Asynchronously retrieves all records that match the specified predicate.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entities. This expression is used to filter the entities that are returned.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of entities that match the specified predicate.
        /// The collection is returned as an enumerable list of entities.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <example>
        /// <code>
        /// // Asynchronously define filtering criteria directly as a parameter
        /// var activeUsers = await _repository.FindAsync(x => x.IsActive);
        /// foreach (var user in activeUsers)
        /// {
        ///     Console.WriteLine($"{user.Name} is active.");
        /// }
        /// </code>
        /// </example>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves all records that match the specified predicate, applying sorting criteria based on the provided order-by specifications.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entities. This expression is used to filter the entities that are returned.
        /// </param>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of entities that match the specified predicate 
        /// and are sorted according to the provided order-by criteria. The collection is returned as an enumerable list of entities.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided or if the <paramref name="orderBys"/> parameter is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Asynchronously define filtering and sorting criteria directly as parameters
        /// var results = await _repository.FindAsync(
        ///     x => x.IsActive,               // Filter by IsActive property
        ///     (x => x.Name, false),         // Sort by Name in ascending order
        ///     (x => x.Population, true)     // Then by Population in descending order
        /// );
        /// foreach (var item in results)
        /// {
        ///     Console.WriteLine($"{item.Name}: {item.Population}");
        /// }
        /// </code>
        /// </example>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FilterAndOrderBy(predicate, orderBys).ToListAsync();
        }


        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FilterAndOrderBy(predicate, orderBys).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        /// <summary>
        /// Asynchronously retrieves all records that match the specified predicate without tracking them in the context.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entities. This expression is used to filter the entities that are returned.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of entities that match the specified predicate 
        /// and are not tracked by the context. The collection is returned as an enumerable list of entities.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <example>
        /// <code>
        /// // Asynchronously define filtering criteria directly as a parameter, without tracking
        /// var activeUsers = await _repository.FindAsyncAsNoTracking(x => x.IsActive);
        /// foreach (var user in activeUsers)
        /// {
        ///     Console.WriteLine($"{user.Name} is active.");
        /// }
        /// </code>
        /// </example>
        public async Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves all records that match the specified predicate, applying sorting criteria based on the provided order-by specifications, without tracking the entities in the context.
        /// </summary>
        /// <param name="predicate">
        /// An expression that defines the criteria for selecting the entities. This expression is used to filter the entities that are returned.
        /// </param>
        /// <param name="orderBys">
        /// A variable number of tuples where each tuple consists of an expression specifying the property to sort by 
        /// and a boolean indicating whether the sorting should be in descending order. 
        /// For example, (x => x.Name, false) sorts by Name in ascending order, while (x => x.Population, true) sorts by Population in descending order.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of entities that match the specified predicate 
        /// and are sorted according to the provided order-by criteria. The collection is returned as an enumerable list of entities, and the entities are not tracked by the context.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="predicate"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if no order-by selectors are provided or if the <paramref name="orderBys"/> parameter is null or empty.
        /// </exception>
        /// <example>
        /// <code>
        /// // Asynchronously define filtering and sorting criteria directly as parameters, without tracking
        /// var results = await _repository.FindAsyncAsNoTracking(
        ///     x => x.IsActive,               // Filter by IsActive property
        ///     (x => x.Name, false),         // Sort by Name in ascending order
        ///     (x => x.Population, true)     // Then by Population in descending order
        /// );
        /// foreach (var item in results)
        /// {
        ///     Console.WriteLine($"{item.Name}: {item.Population}");
        /// }
        /// </code>
        /// </example>
        public async Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FilterAndOrderBy(predicate, orderBys).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FilterAndOrderBy(predicate, orderBys).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        }


        #endregion

        #region Add

        /// <summary>
        /// Adds a new entity to the database context and returns the added entity.
        /// </summary>
        /// <param name="entity">
        /// The entity to add to the database context. This entity is tracked by the context and will be inserted into the database when `SaveChanges` is called.
        /// </param>
        /// <returns>
        /// The added entity, which is the same as the input entity. The entity will be in the Added state in the context until changes are saved.
        /// </returns>
        /// <example>
        /// <code>
        /// // Create a new entity
        /// var newEntity = new MyEntity { Name = "New Entity", Value = 123 };
        ///
        /// // Add the new entity to the context
        /// var addedEntity = _repository.Add(newEntity);
        ///
        /// // Save changes to persist the new entity in the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

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
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Adds a collection of entities to the database context and returns the added entities.
        /// </summary>
        /// <param name="entities">
        /// An <see cref="IEnumerable{T}"/> of entities to add to the database context. These entities will be tracked by the context and will be inserted into the database when `SaveChanges` is called.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token to observe while performing the operation. This parameter is not used in this method but is included for consistency with asynchronous patterns. The default value is <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// The same collection of entities that were added to the context. These entities will be in the Added state in the context until changes are saved.
        /// </returns>
        /// <example>
        /// <code>
        /// // Create a collection of new entities
        /// var newEntities = new List<MyEntity>
        /// {
        ///     new MyEntity { Name = "Entity 1", Value = 100 },
        ///     new MyEntity { Name = "Entity 2", Value = 200 }
        /// };
        ///
        /// // Add the new entities to the context
        /// var addedEntities = _repository.AddRange(newEntities);
        ///
        /// // Save changes to persist the new entities in the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        public IEnumerable<T> AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().AddRange(entities);
            return entities;
        }

        /// <summary>
        /// Asynchronously adds a collection of entities to the database context and returns the added entities.
        /// </summary>
        /// <param name="entities">
        /// An <see cref="IEnumerable{T}"/> of entities to add to the database context. These entities will be tracked by the context and will be inserted into the database when `SaveChanges` is called.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token to observe while waiting for the asynchronous operation to complete. This allows the operation to be canceled if needed. The default value is <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the same collection of entities that were added to the context. These entities will be in the Added state in the context until changes are saved.
        /// </returns>
        /// <example>
        /// <code>
        /// // Create a collection of new entities
        /// var newEntities = new List<MyEntity>
        /// {
        ///     new MyEntity { Name = "Entity 1", Value = 100 },
        ///     new MyEntity { Name = "Entity 2", Value = 200 }
        /// };
        ///
        /// // Asynchronously add the new entities to the context
        /// var addedEntities = await _repository.AddRangeAsync(newEntities);
        ///
        /// // Save changes to persist the new entities in the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the operation is canceled by the <paramref name="cancellationToken"/>.
        /// </exception>
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
            return entities;
        }


        #endregion

        #region Update

        /// <summary>
        /// Updates an existing entity in the database context and returns the updated entity.
        /// </summary>
        /// <param name="entity">
        /// The entity to update. This entity should already be attached to the database context or should be tracked by the context. Its state will be updated to reflect any changes.
        /// </param>
        /// <returns>
        /// The updated entity. This entity will be in the Modified state in the context until `SaveChanges` is called to persist the changes to the database.
        /// </returns>
        /// <example>
        /// <code>
        /// // Retrieve an existing entity
        /// var existingEntity = _dbContext.Entities.Find(entityId);
        ///
        /// // Modify the entity's properties
        /// existingEntity.Name = "Updated Name";
        ///
        /// // Update the entity in the context
        /// var updatedEntity = _repository.Update(existingEntity);
        ///
        /// // Save changes to persist the updated entity in the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        /// <remarks>
        /// The `Update` method marks the entity as modified, and the changes will be saved to the database when `SaveChanges` is called. Ensure that the entity's primary key value is set correctly before calling this method.
        /// </remarks>
        public T Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return entity;
        }

        /// <summary>
        /// Asynchronously updates an existing entity in the database context and returns the updated entity.
        /// </summary>
        /// <param name="entity">
        /// The entity to update. This entity should already be attached to the database context or should be tracked by the context. Its state will be updated to reflect any changes.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the updated entity. This entity will be in the Modified state in the context until `SaveChangesAsync` is called to persist the changes to the database.
        /// </returns>
        /// <example>
        /// <code>
        /// // Retrieve an existing entity
        /// var existingEntity = await _dbContext.Entities.FindAsync(entityId);
        ///
        /// // Modify the entity's properties
        /// existingEntity.Name = "Updated Name";
        ///
        /// // Asynchronously update the entity in the context
        /// var updatedEntity = await _repository.UpdateAsync(existingEntity);
        ///
        /// // Save changes asynchronously to persist the updated entity in the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        /// <remarks>
        /// The `UpdateAsync` method marks the entity as modified, and the changes will be saved to the database when `SaveChangesAsync` is called. The use of `Task.FromResult` is to fulfill the asynchronous pattern without actually performing any asynchronous work, as the update operation itself is synchronous.
        /// </remarks>
        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return await Task.FromResult(entity);
        }

        /// <summary>
        /// Updates a range of entities in the database context and returns the updated entities.
        /// </summary>
        /// <param name="entities">
        /// A list of entities to update. These entities should already be attached to the database context or tracked by the context. Their states will be updated to reflect any changes.
        /// </param>
        /// <returns>
        /// The list of updated entities. These entities will be in the Modified state in the context until `SaveChanges` is called to persist the changes to the database.
        /// </returns>
        /// <example>
        /// <code>
        /// // Retrieve a list of entities to update
        /// var entitiesToUpdate = _dbContext.Entities.Where(e => e.SomeCondition).ToList();
        ///
        /// // Modify the entities' properties
        /// foreach (var entity in entitiesToUpdate)
        /// {
        ///     entity.Name = "Updated Name";
        /// }
        ///
        /// // Update the entities in the context
        /// var updatedEntities = _repository.UpdateRange(entitiesToUpdate);
        ///
        /// // Save changes to persist the updated entities in the database
        /// _dbContext.SaveChanges();
        /// </code>
        /// </example>
        /// <remarks>
        /// The `UpdateRange` method marks all the entities in the list as modified, and the changes will be saved to the database when `SaveChanges` is called. Ensure that each entity's primary key value is set correctly before calling this method.
        /// </remarks>
        public IEnumerable<T> UpdateRange(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return entities;
        }

        /// <summary>
        /// Asynchronously updates a range of entities in the database context and returns the updated entities.
        /// </summary>
        /// <param name="entities">
        /// A list of entities to update. These entities should already be attached to the database context or tracked by the context. Their states will be updated to reflect any changes.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the list of updated entities. These entities will be in the Modified state in the context until `SaveChangesAsync` is called to persist the changes to the database.
        /// </returns>
        /// <example>
        /// <code>
        /// // Retrieve a list of entities to update
        /// var entitiesToUpdate = _dbContext.Entities.Where(e => e.SomeCondition).ToList();
        ///
        /// // Modify the entities' properties
        /// foreach (var entity in entitiesToUpdate)
        /// {
        ///     entity.Name = "Updated Name";
        /// }
        ///
        /// // Asynchronously update the entities in the context
        /// var updatedEntities = await _repository.UpdateRangeAsync(entitiesToUpdate);
        ///
        /// // Save changes asynchronously to persist the updated entities in the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        /// <remarks>
        /// The `UpdateRangeAsync` method marks all entities in the list as modified. Although this method returns a completed task using `Task.FromResult`, the actual update operation is synchronous. The updated entities will be in the `Modified` state in the context until `SaveChangesAsync` is called to persist the changes to the database. Ensure that each entity's primary key value is set correctly before calling this method.
        /// </remarks>
        public async Task<IEnumerable<T>> UpdateRangeAsync(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return await Task.FromResult<IEnumerable<T>>(entities);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Removes a single entity from the database context.
        /// </summary>
        /// <param name="entity">
        /// The entity to remove. The entity should be attached to the database context. If the entity is not tracked by the context, this method will not affect the database.
        /// </param>
        /// <remarks>
        /// This method marks the specified entity for deletion. The entity will be removed from the context's change tracker and, once `SaveChanges` is called, it will be deleted from the database. If you need to ensure that the entity is actually removed from the database, call `SaveChanges` or `SaveChangesAsync` after calling this method.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve the entity to remove
        /// var entityToRemove = _dbContext.Entities.Find(entityId);
        ///
        /// // Remove the entity from the context
        /// _repository.Remove(entityToRemove);
        ///
        /// // Save changes to persist the removal to the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        public void Remove(T entity) =>
            _dbContext.Set<T>().Remove(entity);

        /// <summary>
        /// Asynchronously removes a single entity from the database context.
        /// </summary>
        /// <param name="entity">
        /// The entity to remove. The entity should be attached to the database context. If the entity is not tracked by the context, this method will not affect the database.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is the entity that was removed. Note that the actual removal from the database will not occur until `SaveChangesAsync` is called.
        /// </returns>
        /// <remarks>
        /// This method marks the specified entity for deletion and returns a completed task. The entity will be removed from the context's change tracker. However, the actual removal from the database will not be performed until `SaveChangesAsync` is called. To persist the changes, you must call `SaveChangesAsync` or `SaveChanges` after invoking this method.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve the entity to remove
        /// var entityToRemove = _dbContext.Entities.Find(entityId);
        ///
        /// // Asynchronously remove the entity from the context
        /// await _repository.RemoveAsync(entityToRemove);
        ///
        /// // Save changes to persist the removal to the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        public async Task RemoveAsync(T entity) =>
            await Task.FromResult(_dbContext.Set<T>().Remove(entity));

        /// <summary>
        /// Removes a range of entities from the database context.
        /// </summary>
        /// <param name="entities">
        /// A collection of entities to remove. The entities should be attached to the database context. If any entity in the collection is not tracked by the context, it will not be removed from the database.
        /// </param>
        /// <remarks>
        /// This method marks the specified entities for deletion. The entities will be removed from the context's change tracker and, once `SaveChanges` is called, they will be deleted from the database. To ensure that the entities are actually removed from the database, call `SaveChanges` or `SaveChangesAsync` after invoking this method.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve the entities to remove
        /// var entitiesToRemove = _dbContext.Entities.Where(e => e.SomeCondition).ToList();
        ///
        /// // Remove the entities from the context
        /// _repository.RemoveRange(entitiesToRemove);
        ///
        /// // Save changes to persist the removal to the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        public void RemoveRange(IEnumerable<T> entities) =>
              _dbContext.Set<T>().RemoveRange(entities);

        /// <summary>
        /// Asynchronously removes a range of entities from the database context.
        /// </summary>
        /// <param name="entities">
        /// A collection of entities to remove. The entities should be attached to the database context. If any entity in the collection is not tracked by the context, it will not be removed from the database.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is a completed task. The actual removal from the database will not occur until `SaveChangesAsync` is called.
        /// </returns>
        /// <remarks>
        /// This method marks the specified entities for deletion and returns a completed task. The entities will be removed from the context's change tracker. However, the actual removal from the database will not be performed until `SaveChangesAsync` is called. To persist the changes, you must call `SaveChangesAsync` or `SaveChanges` after invoking this method.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Retrieve the entities to remove
        /// var entitiesToRemove = _dbContext.Entities.Where(e => e.SomeCondition).ToList();
        ///
        /// // Asynchronously remove the entities from the context
        /// await _repository.RemoveRangeAsync(entitiesToRemove);
        ///
        /// // Save changes to persist the removal to the database
        /// await _dbContext.SaveChangesAsync();
        /// </code>
        /// </example>
        public async Task RemoveRangeAsync(IEnumerable<T> entities) =>
            await Task.FromResult(_dbContext.Set<IEnumerable<T>>().Remove(entities));

        #endregion

        #region count

        /// <summary>
        /// Counts the number of entities in the database set.
        /// </summary>
        /// <returns>
        /// The number of entities in the set. This is the total count of entities that are currently tracked by the context.
        /// </returns>
        /// <remarks>
        /// This method returns the total number of entities in the database set. It performs a count operation directly against the database and does not require materializing the entities. This operation is executed synchronously.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Get the total number of entities in the set
        /// int entityCount = _repository.Count();
        /// </code>
        /// </example>
        public int Count()
        {
            return _dbContext.Set<T>().Count();
        }

        /// <summary>
        /// Counts the number of entities in the database set that match the specified predicate.
        /// </summary>
        /// <param name="where">
        /// An expression that defines the criteria to filter the entities to be counted. The predicate is applied to each entity, and only entities that satisfy the condition are included in the count.
        /// </param>
        /// <returns>
        /// The number of entities that match the specified predicate. This is the count of entities that meet the criteria defined by the predicate.
        /// </returns>
        /// <remarks>
        /// This method executes a count operation directly against the database and does not require materializing the entities. It performs the counting based on the condition specified in the `where` predicate and is executed synchronously.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Count the number of entities where the Name property equals "John"
        /// int count = _repository.Count(x => x.Name == "John");
        /// </code>
        /// </example>
        public int Count(Expression<Func<T, bool>> where)
        {
            return _dbContext.Set<T>().Count(where);
        }

        /// <summary>
        /// Asynchronously counts the number of entities in the database set.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the number of entities in the set. This count represents the total number of entities currently tracked by the context.
        /// </returns>
        /// <remarks>
        /// This method performs the counting operation asynchronously and directly against the database. It returns the total number of entities in the database set. It does not require materializing the entities and is suitable for use in asynchronous programming scenarios.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously get the total number of entities in the set
        /// int entityCount = await _repository.CountAsync();
        /// </code>
        /// </example>
        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        /// <summary>
        /// Asynchronously counts the number of entities in the database set that match the specified predicate.
        /// </summary>
        /// <param name="where">
        /// An expression that defines the criteria to filter the entities to be counted. Only entities that satisfy the condition specified in the predicate are included in the count.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the number of entities that match the specified predicate.
        /// </returns>
        /// <remarks>
        /// This method performs the counting operation asynchronously and directly against the database. It uses the specified predicate to filter entities before counting. It does not require materializing the entities and is suitable for use in asynchronous programming scenarios.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously count the number of entities where the Name property equals "John"
        /// int count = await _repository.CountAsync(x => x.Name == "John");
        /// </code>
        /// </example>
        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return await _dbContext.Set<T>().CountAsync(where);
        }

        /// <summary>
        /// Counts the number of entities in the database set and returns the result as a <see cref="long"/>.
        /// </summary>
        /// <returns>
        /// The total number of entities in the set, represented as a <see cref="long"/>. This count represents the number of entities currently tracked by the context.
        /// </returns>
        /// <remarks>
        /// This method performs the counting operation synchronously and returns the total number of entities as a <see cref="long"/>. It is useful when the count exceeds the range of an <see cref="int"/> and a larger data type is required.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Get the total number of entities in the set as a long
        /// long entityCount = _repository.CountLong();
        /// </code>
        /// </example>
        public long CountLong()
        {
            return _dbContext.Set<T>().LongCount();
        }

        /// <summary>
        /// Counts the number of entities in the database set that match the specified predicate and returns the result as a <see cref="long"/>.
        /// </summary>
        /// <param name="where">
        /// An expression that defines the criteria to filter the entities to be counted. Only entities that satisfy the condition specified in the predicate are included in the count.
        /// </param>
        /// <returns>
        /// The total number of entities that match the specified predicate, represented as a <see cref="long"/>. This count represents the number of entities currently tracked by the context that meet the predicate's criteria.
        /// </returns>
        /// <remarks>
        /// This method performs the counting operation synchronously based on the given predicate and returns the count as a <see cref="long"/>. It is useful when the count exceeds the range of an <see cref="int"/> and a larger data type is required. The predicate is used to filter entities before counting.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Get the count of entities where the Name property equals "John" as a long
        /// long count = _repository.CountLong(x => x.Name == "John");
        /// </code>
        /// </example>
        public long CountLong(Expression<Func<T, bool>> where)
        {
            return _dbContext.Set<T>().LongCount(where);
        }

        /// <summary>
        /// Asynchronously counts the number of entities in the database set and returns the result as a <see cref="long"/>.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the total number of entities in the set, represented as a <see cref="long"/>. This count represents the number of entities currently tracked by the context.
        /// </returns>
        /// <remarks>
        /// This method performs the counting operation asynchronously and returns the total count as a <see cref="long"/>. It is useful for scenarios where the count might exceed the range of an <see cref="int"/> and a larger data type is required. Asynchronous execution helps avoid blocking the calling thread while the count is being computed.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously get the total number of entities in the set as a long
        /// long entityCount = await _repository.CountLongAsync();
        /// </code>
        /// </example>
        public async Task<long> CountLongAsync()
        {
            return await _dbContext.Set<T>().LongCountAsync();
        }

        /// <summary>
        /// Asynchronously counts the number of entities in the database set that match the specified predicate and returns the result as a <see cref="long"/>.
        /// </summary>
        /// <param name="where">
        /// An expression that defines the criteria to filter the entities to be counted. Only entities that satisfy the condition specified in the predicate are included in the count.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the total number of entities that match the specified predicate, represented as a <see cref="long"/>. This count represents the number of entities currently tracked by the context that meet the predicate's criteria.
        /// </returns>
        /// <remarks>
        /// This method performs the counting operation asynchronously based on the given predicate and returns the count as a <see cref="long"/>. It is useful when the count might exceed the range of an <see cref="int"/> and a larger data type is required. Asynchronous execution helps avoid blocking the calling thread while the count is being computed. The predicate is used to filter entities before counting.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously get the count of entities where the Name property equals "John" as a long
        /// long count = await _repository.CountLongAsync(x => x.Name == "John");
        /// </code>
        /// </example>
        public async Task<long> CountLongAsync(Expression<Func<T, bool>> where)
        {
            return await _dbContext.Set<T>().LongCountAsync(where);
        }

        #endregion

        #region Any

        /// <summary>
        /// Determines whether any entities exist in the database set.
        /// </summary>
        /// <returns>
        /// A <see cref="bool"/> indicating whether there are any entities in the set. Returns <c>true</c> if there is at least one entity; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method checks if there are any entities in the database set without applying any filtering or sorting criteria. It is a simple and efficient way to determine if the set contains any records.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Check if there are any entities in the database set
        /// bool hasEntities = _repository.Any();
        /// </code>
        /// </example>
        public bool Any()
        {
            return _dbContext.Set<T>().Any();
        }

        /// <summary>
        /// Determines whether any entities in the database set match the specified predicate.
        /// </summary>
        /// <param name="where">
        /// An expression specifying the condition to be checked against each entity in the set.
        /// </param>
        /// <returns>
        /// A <see cref="bool"/> indicating whether any entities in the set satisfy the given condition. Returns <c>true</c> if at least one entity matches the predicate; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="where"/> parameter is <c>null</c>.
        /// </exception>
        /// <remarks>
        /// This method checks if there are any entities in the database set that satisfy the specified condition. It efficiently determines the existence of matching records without retrieving them.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Check if there are any entities with a Name starting with "A"
        /// bool exists = _repository.Any(x => x.Name.StartsWith("A"));
        /// </code>
        /// </example>
        public bool Any(Expression<Func<T, bool>> where)
        {
            return _dbContext.Set<T>().Any(where);
        }

        /// <summary>
        /// Asynchronously determines whether any entities exist in the database set.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> representing the asynchronous operation. The task result is <c>true</c> if there is at least one entity in the set; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method asynchronously checks if there are any entities in the database set without applying any filtering or sorting criteria. It is an efficient way to determine if the set contains any records.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously check if there are any entities in the database set
        /// bool hasEntities = await _repository.AnyAsync();
        /// </code>
        /// </example>
        public async Task<bool> AnyAsync()
        {
            return await _dbContext.Set<T>().AnyAsync();
        }

        /// <summary>
        /// Asynchronously determines whether any entities in the database set satisfy the specified condition.
        /// </summary>
        /// <param name="where">
        /// An expression that defines the condition to filter the entities. This predicate is used to determine which entities to consider in the count.
        /// </param>
        /// <returns>
        /// A <see cref="Task{Boolean}"/> representing the asynchronous operation. The task result is <c>true</c> if at least one entity matches the condition; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method asynchronously checks if there are any entities in the database set that meet the specified condition. It is useful for checking the existence of records that satisfy certain criteria.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Asynchronously check if there are any entities that satisfy a specific condition
        /// bool exists = await _repository.AnyAsync(x => x.IsActive);
        /// </code>
        /// </example>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await _dbContext.Set<T>().AnyAsync(where);
        }
        #endregion


        #region SaveChanges
        /// <summary>
        /// Saves all changes made in the context to the database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database. This includes the number of entities that were added, updated, or deleted.
        /// </returns>
        /// <remarks>
        /// This method is used to persist changes made in the current context to the database. It will commit all modifications tracked by the context, such as adding, updating, or deleting entities.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Save all changes made in the context to the database
        /// int changes = _dbContext.SaveChanges();
        /// Console.WriteLine($"Number of changes saved: {changes}");
        /// </code>
        /// </example>
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in the context to the database.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database. This includes the number of entities that were added, updated, or deleted.
        /// </returns>
        /// <remarks>
        /// This method is used to asynchronously commit all modifications tracked by the context to the database. It handles adding, updating, or deleting entities in an asynchronous manner, allowing for non-blocking operations.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Save all changes made in the context to the database asynchronously
        /// int changes = await _dbContext.SaveChangesAsync();
        /// Console.WriteLine($"Number of changes saved: {changes}");
        /// </code>
        /// </example>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Transaction Management

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        /// <returns>The transaction object.</returns>
        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
            return _transaction;
        }

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
        /// Commits the current transaction.
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                SaveChanges();
                _transaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
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
        /// Rolls back the current transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                }
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

        public Expression<Func<T, bool>> GeneratePredicate(Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }


    }
}
