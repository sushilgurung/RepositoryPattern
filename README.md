
# Repository Pattern with Entity Framework Core

## Overview

The Repository Pattern provides a way to encapsulate data access logic and abstract away the details of data storage and retrieval. This approach promotes separation of concerns, making your application more maintainable and testable.


## Target Framework
This project is built with .NET 7.0 and uses Entity Framework Core version 7.0.20.## Installation

Install with .NET CLI
```bash
 dotnet add package Gurung.RepositoryPattern --version 7.0.1
```
Install with Package Manager
```bash
 NuGet\Install-Package Gurung.RepositoryPattern -Version 7.0.1
```

    
## Example Usage

### Configuring Services in Startup

To add the repository service to the dependency injection container, use the `RepositoryServiceRegistration.ConfigureServices` method in the `Startup` or `Pragram` class. Here's how you can do it:

### Example in Startup.cs

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register your DbContext
        services.AddDbContext<DbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Register the repository service
        RepositoryServiceRegistration.ConfigureServices(services);
    }
}
```

### Example in Program.cs
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Gurung.RepositoryPattern; // Update namespace

var builder = WebApplication.CreateBuilder(args);

// Register your DbContext
builder.Services.AddDbContext<DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the repository service
RepositoryServiceRegistration.ConfigureServices(builder.Services);

var app = builder.Build();
app.Run();
```
## Documentation

## Repository Interface

The `IRepository<T>` interface offers a wide range of methods for interacting with your data. Below is a summary of the available methods:


### Queryable

- [`IQueryable<T> Queryable { get; }`](#queryable-1)
- [`IQueryable<T> GetQueryable()`](#using-getqueryable)

### GeneratePredicate

- [`Expression<Func<T, bool>> GeneratePredicate()`](#generatepredicate-1)

### GetAll

- `IEnumerable<T> GetAll()`
- `IEnumerable<T> GetAllAsNoTracking()`
- `IEnumerable<T> GetAll(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `IEnumerable<T> GetAllAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<IEnumerable<T>> GetAllAsync()`
- `Task<IEnumerable<T>> GetAllAsNoTrackingAsync()`
- `Task<IEnumerable<T>> GetAllAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<IEnumerable<T>> GetAllAsNoTrackingAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`

- `IEnumerable<T> GetAll(int pageNumber, int pageSize)`
- `IEnumerable<T> GetAllAsNoTracking(int pageNumber, int pageSize)`
- `IEnumerable<T> GetAll(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `IEnumerable<T> GetAllAsNoTracking(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize)`
- `Task<IEnumerable<T>> GetAllAsNoTrackingAsync(int pageNumber, int pageSize)`
- `Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<IEnumerable<T>> GetAllAsNoTrackingAsync(int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`

### GetById

- `T GetById(int id)`
- `T GetById(long id)`
- `Task<T> GetByIdAsync(int id)`
- `Task<T> GetByIdAsync(long id)`
- `T GetByIdAsNoTracking(int id)`
- `T GetByIdAsNoTracking(long id)`
- `Task<T> GetByIdAsNoTrackingAsync(int id)`
- `Task<T> GetByIdAsNoTrackingAsync(long id)`


- `T GetById(string id)`
- `Task<T> GetByIdAsync(string id)`
- `T GetByIdAsNoTracking(string id)`
- `Task<T> GetByIdAsNoTrackingAsync(string id)`

- `T GetById(Guid id)`
- `Task<T> GetByIdAsync(Guid id)`
- `T GetByIdAsNoTracking(Guid id)`
- `Task<T> GetByIdAsNoTrackingAsync(Guid id)`


### FirstOrDefault

- `T FirstOrDefault()`
- `Task<T> FirstOrDefaultAsync()`
- `T FirstOrDefaultAsNoTracking()`
- `Task<T> FirstOrDefaultAsyncAsNoTracking()`
- `T FirstOrDefault(Expression<Func<T, bool>> predicate)`
- `Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)`
- `T FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate)`
- `Task<T> FirstOrDefaultAsyncAsNoTracking(Expression<Func<T, bool>> predicate)`

- ` T FirstOrDefault(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<T> FirstOrDefaultAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `T FirstOrDefaultAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<T> FirstOrDefaultAsyncAsNoTracking(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`


- `T FirstOrDefault(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `T FirstOrDefaultAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<T> FirstOrDefaultAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`

### Find

- `IEnumerable<T> Find(Expression<Func<T, bool>> predicate)`
- `IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params (Expression<Func<T,  object>> KeySelector, bool Descending)[] orderBys)`
- `IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`

- `IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate)`
- `IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`

- `Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)`
- `Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`

- `Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate)`
- `Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`
- `Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)`



### Add

- `T Add(T entity)`
- `Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)`
- `IEnumerable<T> AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)`
- `Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)`

### Update

- `T Update(T entity)`
- `Task<T> UpdateAsync(T entity)`
- `IEnumerable<T> UpdateRange(List<T> entities)`
- `Task<IEnumerable<T>> UpdateRangeAsync(List<T> entities)`

### Delete

- `void Remove(T entity)`
- `Task RemoveAsync(T entity)`
- `void RemoveRange(IEnumerable<T> entities)`
- `Task RemoveRangeAsync(IEnumerable<T> entities)`

### Count

- `int Count()`
- `int Count(Expression<Func<T, bool>> where)`
- `Task<int> CountAsync()`
- `Task<int> CountAsync(Expression<Func<T, bool>> where)`

### Any

- `bool Any()`
- `bool Any(Expression<Func<T, bool>> where)`
- `Task<bool> AnyAsync()`
- `Task<bool> AnyAsync(Expression<Func<T, bool>> where)`

### Save Changes

- `int SaveChanges()`
- `Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)`

### Transaction Management

- `IDbContextTransaction BeginTransaction()`
- `Task<IDbContextTransaction> BeginTransactionAsync()`
- `Task CommitTransactionAsync()`
- `Task RollbackTransactionAsync()`





### Using Queryable

#### Queryable

```csharp
        var query = from area in _areasRepository.Queryable 
                    join country in _countryRepository.Queryable 
                    on area.CountryId equals country.Id
                    join state in _stateRepository.Queryable 
                    on area.StateId equals state.Id
                    where area.Id != 0 && area.IsDeleted == request.IsDeleted
                    orderby area.Location
                    select new GetAllAreaViewModel
                    {
                        Id = area.Id,
                        Location = area.Location,
                        Country = country.CountryName,
                        State = state.StateName,
                        Status = area.Status,
                        StatusNews = area.StatusNews,
                        IsActive = area.Status 
                    };
        IEnumerable<GetAllAreaViewModel> getAllAreaViewModels =await query.ToListAsync();
```

#### Using GetQueryable()
```csharp
        var query = from area in _areasRepository.GetQueryable()
                    join country in _countryRepository.GetQueryable()
                    on area.CountryId equals country.Id
                    join state in _stateRepository.GetQueryable()
                    on area.StateId equals state.Id
                    where area.Id != 0 && area.IsDeleted == request.IsDeleted
                    orderby area.Location
                    select new GetAllAreaViewModel
                    {
                        Id = area.Id,
                        Location = area.Location,
                        Country = country.CountryName,
                        State = state.StateName,
                        Status = area.Status,
                        StatusNews = area.StatusNews,
                        IsActive = area.Status 
                    };
        IEnumerable<GetAllAreaViewModel> getAllAreaViewModels =await query.ToListAsync();
```
## GeneratePredicate 
The GeneratePredicate method allows the creation of dynamic filters using LINQ expressions (Expression<Func<T, bool>>). This flexibility helps you apply conditions dynamically, such as filtering data based on specific criteria.
```csharp
public Task<PagedList<Product>> GetExpensiveProducts(int pageNumber, int pageSize)
{
       // Step 1: Generate a predicate for products with Price > 100
        Expression<Func<Product, bool>> predicate = p => p.Price > 100;

        // Step 2: Use the GeneratePredicate method to get the predicate
        var filter = _productRepository.GeneratePredicate(predicate);

        // Step 3: Get the count of products that match the filter
        var count = await _productRepository.CountAsync(predicate);

        // Step 4: Fetch products asynchronously using pagination and the predicate
        var productList= await _productRepository.FindAsync(pageNumber, pageSize, filter);
        return await PagedList<Product>(productList, pageNumber, pageSize);
}
```

### Using GetAll

Retrieves all entities from the database.
#### GetAll
```csharp
public IEnumerable<MyEntity> GetAllEntities()
{
    // retrieve all entities of type MyEntity from the database
    var allEntities =  _countryRepository.GetAll();
    foreach (var entity in allEntities)
    {
        Console.WriteLine(entity.Name);
    }
}
```

#### GetAllAsNoTracking
Retrieves all entities of the specified type `T` from the data source without tracking them in the context.
```csharp
public IEnumerable<MyEntity> GetAllEntitiesAsNoTracking()
{
    // Retrieve all entities of type MyEntity from the database without tracking
    var allEntitiesNoTracking = _countryRepository.GetAllAsNoTracking();
    foreach (var entity in allEntitiesNoTracking)
    {
        Console.WriteLine(entity.Name);
    }
}
```
#### GetAll with Ordering
Retrieves all entities of the specified type T with optional ordering.
```csharp
public IEnumerable<MyEntity> GetAllEntitiesOrdered()
{
    // Retrieve all entities of type MyEntity from the database, ordered by Name (ascending) and Population (descending)
    var orderedEntities = _countryRepository.GetAll(
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );

    foreach (var entity in orderedEntities)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
}
```

#### GetAllAsNoTrackingAsync with Ordering
Asynchronously retrieves all entities of the specified type T without tracking and with optional ordering.
```csharp
public async Task<IEnumerable<MyEntity>> GetAllEntitiesAsNoTrackingAsyncOrdered()
{
    // Asynchronously retrieve all entities of type MyEntity from the database without tracking, ordered by Name (ascending) and Population (descending)
    var orderedEntitiesNoTracking = await _countryRepository.GetAllAsNoTrackingAsync(
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );

    foreach (var entity in orderedEntitiesNoTracking)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
}
```

#### GetAllAsync
```csharp
public async Task<IEnumerable<MyEntity>> GetAllEntitiesAsync()
{
    // Asynchronously retrieve all entities of type MyEntity from the database
    var allEntities = await _countryRepository.GetAllAsync();
    foreach (var entity in allEntities)
    {
        Console.WriteLine(entity.Name);
    }
}
```

#### GetAllAsNoTrackingAsync
Asynchronously retrieves all entities of the specified type T from the data source without tracking.
```csharp
public async Task<IEnumerable<MyEntity>> GetAllEntitiesAsNoTrackingAsync()
{
    // Asynchronously retrieve all entities of type MyEntity from the database without tracking
    var allEntitiesNoTracking = await _countryRepository.GetAllAsNoTrackingAsync();
    foreach (var entity in allEntitiesNoTracking)
    {
        Console.WriteLine(entity.Name);
    }
}
```

#### GetAllAsync with Ordering
Asynchronously retrieves all entities of the specified type T with optional ordering.
```csharp
public async Task<IEnumerable<MyEntity>> GetAllEntitiesAsyncOrdered()
{
    // Asynchronously retrieve all entities of type MyEntity from the database, ordered by Name (ascending) and Population (descending)
    var orderedEntities = await _countryRepository.GetAllAsync(
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );

    foreach (var entity in orderedEntities)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
}
```
#### GetAllAsNoTrackingAsync with Ordering
Asynchronously retrieves all entities of the specified type T without tracking and with optional ordering.
```csharp
public async Task<IEnumerable<MyEntity>> GetAllEntitiesAsNoTrackingAsyncOrdered()
{
    // Asynchronously retrieve all entities of type MyEntity from the database without tracking, ordered by Name (ascending) and Population (descending)
    var orderedEntitiesNoTracking = await _countryRepository.GetAllAsNoTrackingAsync(
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );

    foreach (var entity in orderedEntitiesNoTracking)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
}
```

#### GetAll (with pagination)
Retrieves a paginated list of entities. Results are tracked by the context (default EF behavior).
```csharp
var products = repository.GetAll(1, 10); 
```

#### GetAllAsNoTracking (with pagination)
Retrieves a paginated list of entities with no-tracking. Results are not tracked by the EF context, which is useful for read-only operations.
```csharp
var products = repository.GetAllAsNoTracking(1, 10); // Get the first page with 10 records, no EF tracking
```

#### GetAll (paginated and sorted results)
```csharp
var sortedProducts = repository.GetAll(1, 10, (p => p.Price, false), (p => p.Name, true)); 
// Get first 10 products sorted by Price ascending, then by Name descending
```
#### GetAll (paginated and sorted results (no-tracking))
```csharp
var sortedProducts = repository.GetAllAsNoTracking(1, 10, (p => p.Price, false), (p => p.Name, true));
```

#### GetAllAsync (with pagination)
```csharp
var products = await repository.GetAllAsync(1, 10); // Fetches the first page of 10 items asynchronously.
```

#### GetAllAsNoTrackingAsync (with pagination)
```csharp
var products = await repository.GetAllAsNoTrackingAsync(1, 10); // Asynchronous, no tracking.
```

#### GetAllAsync (with sorting and pagination)
```csharp
var asyncSortedProducts = await repository.GetAllAsync(1, 10, (p => p.Price, false), (p => p.Name, true));
```

#### GetAllAsNoTrackingAsync (with sorting and pagination)
```csharp
var asyncSortedProducts = await repository.GetAllAsNoTrackingAsync(1, 10, (p => p.Price, false), (p => p.Name, true));
```


## Using GetById
Retrieves a single entity of type T from the database using its primary key (PK),  types: string, int, long, and Guid. This method queries the database context to find the entity that matches the given primary key value.

#### GetById (int)(long)(string)(Guid)

```csharp
var entityInt = _countryRepository.GetById(123);
var entityLong = _countryRepository.GetById(9876543210L);
var entityString = _countryRepository.GetById("abc123");
var entityGuid = _countryRepository.GetById(Guid.Parse("31ab6f9a-bb12-4359-9a2d-c92027ba50c6"));
```

### Using `FirstOrDefaultAsync`

#### FirstOrDefault
Retrieves the first entity of the specified type `T` from the data source.
```csharp
public MyEntity GetFirstEntity()
{
    // Retrieve the first entity of type MyEntity from the database
    var firstEntity = _countryRepository.FirstOrDefault();
    Console.WriteLine(firstEntity.Name);
    return firstEntity;
}
```

#### FirstOrDefaultAsync
Asynchronously retrieves the first entity of the specified type T from the data source.
```csharp
public async Task<MyEntity> GetFirstEntityAsync()
{
    // Asynchronously retrieve the first entity of type MyEntity from the database
    var firstEntity = await _countryRepository.FirstOrDefaultAsync();
    Console.WriteLine(firstEntity.Name);
    return firstEntity;
}
```

#### FirstOrDefaultAsNoTracking
Retrieves the first entity of the specified type T from the data source without tracking.
```csharp
public MyEntity GetFirstEntityAsNoTracking()
{
    // Retrieve the first entity of type MyEntity from the database without tracking
    var firstEntityNoTracking = _countryRepository.FirstOrDefaultAsNoTracking();
    Console.WriteLine(firstEntityNoTracking.Name);
    return firstEntityNoTracking;
}
```

#### FirstOrDefaultAsyncAsNoTracking
Asynchronously retrieves the first entity of the specified type T from the data source without tracking.
```csharp
public async Task<MyEntity> GetFirstEntityAsyncAsNoTracking()
{
    // Asynchronously retrieve the first entity of type MyEntity from the database without tracking
    var firstEntityNoTracking = await _countryRepository.FirstOrDefaultAsyncAsNoTracking();
    Console.WriteLine(firstEntityNoTracking.Name);
    return firstEntityNoTracking;
}
```
#### FirstOrDefault with Predicate
Retrieves the first entity of the specified type T that matches a given predicate.
```csharp
public MyEntity GetFirstActiveEntity()
{
    // Retrieve the first active entity of type MyEntity from the database
    var firstActiveEntity = _countryRepository.FirstOrDefault(x => x.IsActive);
    Console.WriteLine(firstActiveEntity.Name);
    return firstActiveEntity;
}
```

#### FirstOrDefaultAsync with Predicate
Asynchronously retrieves the first entity of the specified type T that matches a given predicate.
```csharp
public async Task<MyEntity> GetFirstActiveEntityAsync()
{
    // Asynchronously retrieve the first active entity of type MyEntity from the database
    var firstActiveEntity = await _countryRepository.FirstOrDefaultAsync(x => x.IsActive);
    Console.WriteLine(firstActiveEntity.Name);
    return firstActiveEntity;
}
```
#### FirstOrDefaultAsNoTracking with Predicate
Retrieves the first entity of the specified type T that matches a given predicate without tracking.
```csharp
public MyEntity GetFirstActiveEntityAsNoTracking()
{
    // Retrieve the first active entity of type MyEntity from the database without tracking
    var firstActiveEntityNoTracking = _countryRepository.FirstOrDefaultAsNoTracking(x => x.IsActive);
    Console.WriteLine(firstActiveEntityNoTracking.Name);
    return firstActiveEntityNoTracking;
}
```
#### FirstOrDefaultAsyncAsNoTracking with Predicate
Asynchronously retrieves the first entity of the specified type T that matches a given predicate without tracking.
```csharp
public async Task<MyEntity> GetFirstActiveEntityAsyncAsNoTracking()
{
    // Asynchronously retrieve the first active entity of type MyEntity from the database without tracking
    var firstActiveEntityNoTracking = await _countryRepository.FirstOrDefaultAsyncAsNoTracking(x => x.IsActive);
    Console.WriteLine(firstActiveEntityNoTracking.Name);
    return firstActiveEntityNoTracking;
}
```

#### FirstOrDefault with Ordering
Retrieves the first entity of the specified type T with optional ordering.
```csharp
public MyEntity GetFirstEntityOrdered()
{
    // Retrieve the first entity of type MyEntity, ordered by Name (ascending) and Population (descending)
    var firstEntityOrdered = _countryRepository.FirstOrDefault(
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );
    Console.WriteLine($"{firstEntityOrdered.Name} - {firstEntityOrdered.Population}");
    return firstEntityOrdered;
}
```
#### FirstOrDefaultAsync with Ordering
Asynchronously retrieves the first entity of the specified type T with optional ordering.
```csharp
 public async Task<MyEntity> GetFirstEntityAsyncOrdered()
{
    // Asynchronously retrieve the first entity of type MyEntity, ordered by Name (ascending) and Population (descending)
    var firstEntityOrdered = await _countryRepository.FirstOrDefaultAsync(
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );
    Console.WriteLine($"{firstEntityOrdered.Name} - {firstEntityOrdered.Population}");
    return firstEntityOrdered;
}
```
#### FirstOrDefaultAsNoTracking with Ordering
Retrieves the first entity of the specified type T without tracking and with optional ordering.
```csharp
public MyEntity GetFirstEntityAsNoTrackingOrdered()
{
    // Retrieve the first entity of type MyEntity without tracking, ordered by Name (ascending) and Population (descending)
    var firstEntityNoTrackingOrdered = _countryRepository.FirstOrDefaultAsNoTracking(
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );
    Console.WriteLine($"{firstEntityNoTrackingOrdered.Name} - {firstEntityNoTrackingOrdered.Population}");
    return firstEntityNoTrackingOrdered;
}
```
#### FirstOrDefaultAsyncAsNoTracking with Ordering
Asynchronously retrieves the first entity of the specified type T without tracking and with optional ordering.
```csharp
public async Task<MyEntity> GetFirstEntityAsyncAsNoTrackingOrdered()
{
    // Asynchronously retrieve the first entity of type MyEntity without tracking, ordered by Name (ascending) and Population (descending)
    var firstEntityNoTrackingOrdered = await _countryRepository.FirstOrDefaultAsyncAsNoTracking(
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );
    Console.WriteLine($"{firstEntityNoTrackingOrdered.Name} - {firstEntityNoTrackingOrdered.Population}");
    return firstEntityNoTrackingOrdered;
}
```

#### FirstOrDefault with Predicate and Ordering
Retrieves the first entity of the specified type T that matches a given predicate with optional ordering.
```csharp
public MyEntity GetFirstActiveEntityOrdered()
{
    // Retrieve the first active entity of type MyEntity, ordered by Name (ascending) and Population (descending)
    var firstActiveEntityOrdered = _countryRepository.FirstOrDefault(
        x => x.IsActive,            // Filtering predicate
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true)    // Then by Population descending
    );
    Console.WriteLine($"{firstActiveEntityOrdered.Name} - {firstActiveEntityOrdered.Population}");
    return firstActiveEntityOrdered;
}
```


### FirstOrDefaultAsync with predicate and order
The `FirstOrDefaultAsync` method allows you to retrieve the first record that matches specified filtering criteria and sorting order. It accepts a filtering predicate and a series of sorting criteria, which can be used to order the results.
```csharp
public async Task<MyEntity> GetTopActiveEntityAsync()
{
    // Call the FirstOrDefaultAsync method with filtering and sorting criteria directly
    var topEntity = await _countryRepository.FirstOrDefaultAsync(
        x => x.IsActive && x.Population > 1000000, // Filtering predicate
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true),   // Then by Population descending
        (x => x.Area, false)         // Finally by Area ascending
    );
    return topEntity;
}

### FirstOrDefaultAsNoTracking  with predicate and order
The `FirstOrDefaultAsync` method allows you to retrieve the first record that matches specified filtering criteria and sorting order. It accepts a filtering predicate and a series of sorting criteria, which can be used to order the results without tracking, with optional ordering..
```csharp
public async Task<MyEntity> GetTopActiveEntityAsync()
{
    // Call the FirstOrDefaultAsync method with filtering and sorting criteria directly
    var topEntity = await _countryRepository.FirstOrDefaultAsNoTracking(
        x => x.IsActive && x.Population > 1000000, // Filtering predicate
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true),   // Then by Population descending
        (x => x.Area, false)         // Finally by Area ascending
    );
    return topEntity;
}
```



### Using `Find`
The `Find` methods are used to retrieve entities that match a given predicate from the data source, with support for optional sorting, pagination, and tracking.

#### Find
```csharp
public IEnumerable<MyEntity> FindEntities(Expression<Func<MyEntity, bool>> predicate)
{
    // Retrieve entities of type MyEntity that match the given predicate
    var foundEntities = _countryRepository.Find(x => x.IsActive);
    foreach (var entity in foundEntities)
    {
        Console.WriteLine(entity.Name);
    }
    return foundEntities;
}
```

#### Find with Ordering
Retrieves entities of the specified type T that match a given predicate, with optional sorting.
```csharp
public IEnumerable<MyEntity> FindEntitiesOrdered()
{
    // Retrieve entities of type MyEntity that match the given predicate and are sorted by Name (ascending) and Population (descending)
    var foundEntities = _countryRepository.Find(
        x => x.IsActive,                    // Filtering predicate
        (x => x.Name, false),               // Sort by Name ascending
        (x => x.Population, true)           // Then by Population descending
    );
    foreach (var entity in foundEntities)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
    return foundEntities;
}
```

#### Find with Pagination
Retrieves entities of the specified type T that match a given predicate, with optional sorting and pagination.
```csharp
public IEnumerable<MyEntity> FindEntitiesPaged()
{
    // Retrieve entities of type MyEntity with filtering, sorting, and pagination
    var foundEntities = _countryRepository.Find(
        x => x.IsActive,                    // Filtering predicate
        1,                                  // Page number
        10,                                 // Page size
        (x => x.Name, false),               // Sort by Name ascending
        (x => x.Population, true)           // Then by Population descending
    );
    foreach (var entity in foundEntities)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
    return foundEntities;
}
```
#### FindAsNoTracking
Retrieves all entities of the specified type T that match a given predicate without tracking.
```csharp
public IEnumerable<MyEntity> FindEntitiesAsNoTracking()
{
    // Retrieve entities of type MyEntity that match the given predicate without tracking
    var foundEntitiesNoTracking = _countryRepository.FindAsNoTracking(x => x.IsActive);
    foreach (var entity in foundEntitiesNoTracking)
    {
        Console.WriteLine(entity.Name);
    }
    return foundEntitiesNoTracking;
}
```

#### FindAsNoTracking with Ordering
Retrieves entities of the specified type T that match a given predicate without tracking, with optional sorting.
```csharp
public IEnumerable<MyEntity> FindEntitiesAsNoTrackingOrdered()
{
    // Retrieve entities of type MyEntity without tracking, sorted by Name (ascending) and Population (descending)
    var foundEntitiesNoTracking = _countryRepository.FindAsNoTracking(
        x => x.IsActive,                    // Filtering predicate
        (x => x.Name, false),               // Sort by Name ascending
        (x => x.Population, true)           // Then by Population descending
    );
    foreach (var entity in foundEntitiesNoTracking)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
    return foundEntitiesNoTracking;
}
```

#### FindAsNoTracking with Pagination
Retrieves entities of the specified type T that match a given predicate without tracking, with optional sorting and pagination.
```csharp
public IEnumerable<MyEntity> FindEntitiesAsNoTrackingPaged()
{
    // Retrieve entities of type MyEntity with filtering, sorting, and pagination without tracking
    var foundEntitiesNoTracking = _countryRepository.FindAsNoTracking(
        x => x.IsActive,                    // Filtering predicate
        1,                                  // Page number
        10,                                 // Page size
        (x => x.Name, false),               // Sort by Name ascending
        (x => x.Population, true)           // Then by Population descending
    );
    foreach (var entity in foundEntitiesNoTracking)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
    return foundEntitiesNoTracking;
}
```

#### FindAsync
Asynchronously retrieves all entities of the specified type T that match a given predicate.
```csharp
public async Task<IEnumerable<MyEntity>> FindEntitiesAsync()
{
    // Asynchronously retrieve entities of type MyEntity that match the given predicate
    var foundEntities = await _countryRepository.FindAsync(x => x.IsActive);
    foreach (var entity in foundEntities)
    {
        Console.WriteLine(entity.Name);
    }
    return foundEntities;
}
```
#### FindAsync with Ordering
Asynchronously retrieves entities of the specified type T that match a given predicate, with optional sorting.
```csharp
public async Task<IEnumerable<MyEntity>> FindEntitiesAsyncOrdered()
{
    // Asynchronously retrieve entities of type MyEntity that match the given predicate, sorted by Name (ascending) and Population (descending)
    var foundEntities = await _countryRepository.FindAsync(
        x => x.IsActive,                    // Filtering predicate
        (x => x.Name, false),               // Sort by Name ascending
        (x => x.Population, true)           // Then by Population descending
    );
    foreach (var entity in foundEntities)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
    return foundEntities;
}
```

#### FindAsync with Pagination
Asynchronously retrieves entities of the specified type T that match a given predicate, with optional sorting and pagination.
```csharp
public async Task<IEnumerable<MyEntity>> FindEntitiesAsyncPaged()
{
    // Asynchronously retrieve entities of type MyEntity with filtering, sorting, and pagination
    var foundEntities = await _countryRepository.FindAsync(
        x => x.IsActive,                    // Filtering predicate
        1,                                  // Page number
        10,                                 // Page size
        (x => x.Name, false),               // Sort by Name ascending
        (x => x.Population, true)           // Then by Population descending
    );
    foreach (var entity in foundEntities)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
    return foundEntities;
}
```

#### FindAsyncAsNoTracking
Asynchronously retrieves all entities of the specified type T that match a given predicate without tracking.
```csharp
public async Task<IEnumerable<MyEntity>> FindEntitiesAsyncAsNoTracking()
{
    // Asynchronously retrieve entities of type MyEntity that match the given predicate without tracking
    var foundEntitiesNoTracking = await _countryRepository.FindAsyncAsNoTracking(x => x.IsActive);
    foreach (var entity in foundEntitiesNoTracking)
    {
        Console.WriteLine(entity.Name);
    }
    return foundEntitiesNoTracking;
}
```

#### FindAsyncAsNoTracking with Ordering
Asynchronously retrieves entities of the specified type T that match a given predicate without tracking, with optional sorting.
```csharp
public async Task<IEnumerable<MyEntity>> FindEntitiesAsyncAsNoTrackingOrdered()
{
    // Asynchronously retrieve entities of type MyEntity without tracking, sorted by Name (ascending) and Population (descending)
    var foundEntitiesNoTracking = await _countryRepository.FindAsyncAsNoTracking(
        x => x.IsActive,                    // Filtering predicate
        (x => x.Name, false),               // Sort by Name ascending
        (x => x.Population, true)           // Then by Population descending
    );
    foreach (var entity in foundEntitiesNoTracking)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
    return foundEntitiesNoTracking;
}
```

#### FindAsyncAsNoTracking with Pagination
Asynchronously retrieves entities of the specified type T that match a given predicate without tracking, with optional sorting and pagination.
```csharp
public async Task<IEnumerable<MyEntity>> FindEntitiesAsyncAsNoTrackingPaged()
{
    // Asynchronously retrieve entities of type MyEntity with filtering, sorting, and pagination without tracking
    var foundEntitiesNoTracking = await _countryRepository.FindAsyncAsNoTracking(
        x => x.IsActive,                    // Filtering predicate
        1,                                  // Page number
        10,                                 // Page size
        (x => x.Name, false),               // Sort by Name ascending
        (x => x.Population, true)           // Then by Population descending
    );
    foreach (var entity in foundEntitiesNoTracking)
    {
        Console.WriteLine($"{entity.Name} - {entity.Population}");
    }
    return foundEntitiesNoTracking;
}
```

## Add and AddRange Methods

#### AddAsync
Asynchronously adds a single entity of type `T` to the data source.
```csharp
public async Task<MyEntity> AddEntityAsync(MyEntity entity)
{
    // Asynchronously add a new entity of type MyEntity
    var addedEntity = await _countryRepository.AddAsync(entity);
      // Save changes to the database
    await _countryRepository.SaveChangesAsync(cancellationToken);
    Console.WriteLine($"Added entity: {addedEntity.Name}");
    return addedEntity;
}
```

#### AddRange
Synchronously adds a range of entities of type T to the data source.
```csharp
public IEnumerable<MyEntity> AddEntitiesRange(IEnumerable<MyEntity> entities)
{
    // Synchronously add a range of entities
    var addedEntities = _countryRepository.AddRange(entities);
      // Save changes to the database
    await _countryRepository.SaveChanges(cancellationToken);
    foreach (var entity in addedEntities)
    {
        Console.WriteLine($"Added entity: {entity.Name}");
    }
    return addedEntities;
}
```

#### AddRangeAsync
Asynchronously adds a range of entities of type T to the data source, with support for cancellation.
```csharp
public async Task<IEnumerable<MyEntity>> AddEntitiesRangeAsync(IEnumerable<MyEntity> entities, CancellationToken cancellationToken = default)
{
    // Asynchronously add a range of entities with a cancellation token
    var addedEntities = await _countryRepository.AddRangeAsync(entities, cancellationToken);
      // Save changes to the database
    await _countryRepository.SaveChangesAsync(cancellationToken);
    foreach (var entity in addedEntities)
    {
        Console.WriteLine($"Added entity: {entity.Name}");
    }
    return addedEntities;
}
```

#### AddAsync with CancellationToken
Asynchronously adds an entity of type T to the data source with a cancellation token.
```csharp
public async Task<MyEntity> AddEntityAsyncWithCancellation(MyEntity entity, CancellationToken cancellationToken)
{
    // Asynchronously add a new entity with a cancellation token
    var addedEntity = await _countryRepository.AddAsync(entity, cancellationToken);
      // Save changes to the database
    await _countryRepository.SaveChangesAsync(cancellationToken);
    Console.WriteLine($"Added entity: {addedEntity.Name}");
    return addedEntity;
}
```

#### AddRange with CancellationToken
Synchronously adds a range of entities of type T to the data source, with support for cancellation.
```csharp
public IEnumerable<MyEntity> AddEntitiesRangeWithCancellation(IEnumerable<MyEntity> entities, CancellationToken cancellationToken)
{
    // Add a range of entities with a cancellation token
    var addedEntities = _countryRepository.AddRange(entities, cancellationToken);
      // Save changes to the database
    await _countryRepository.SaveChangesAsync(cancellationToken);
    foreach (var entity in addedEntities)
    {
        Console.WriteLine($"Added entity: {entity.Name}");
    }
    return addedEntities;
}
```

## Update

#### Update
Synchronously updates a single entity of type T in the database context. This method marks the entity as modified, and the changes will be persisted when SaveChanges is called.
```csharp
// Retrieve the entity to update
var entityToUpdate = _countryRepository.GetById(100);

// Modify the entity
entityToUpdate.Name = "Updated Name";

// Update the entity
var updatedEntity = _countryRepository.Update(entityToUpdate);

// Save changes to the database
_countryRepository.SaveChanges();
```

#### UpdateAsync
Asynchronously updates a single entity of type T in the database context. This method marks the entity as modified and will persist changes once SaveChangesAsync is called.
```csharp
// Retrieve the entity to update
var entityToUpdate = await _countryRepository.GetById(10002);

// Modify the entity
entityToUpdate.Name = "Updated Name";

// Asynchronously update the entity
var updatedEntity = await _countryRepository.UpdateAsync(entityToUpdate);

// Save changes asynchronously to the database
await _countryRepository.SaveChangesAsync();
```

#### UpdateRange
Synchronously updates a range of entities of type T in the database context. This method marks each entity in the collection as modified and will persist the changes once SaveChanges is called.
```csharp
// Retrieve the entities to update
var entitiesToUpdate = _countryRepository.Find(e => e.SomeCondition);

// Modify each entity
foreach (var entity in entitiesToUpdate)
{
    entity.Name = "Updated Name";
}

// Update the entities in the repository
var updatedEntities = _countryRepository.UpdateRange(entitiesToUpdate);

// Save changes to the database
_countryRepository.SaveChanges();
```
#### UpdateRangeAsync
Asynchronously updates a range of entities of type T in the database context. This method marks each entity in the collection as modified and will persist the changes once SaveChangesAsync is called.
```csharp
// Retrieve the entities to update
var entitiesToUpdate = await _countryRepository.Find(e => e.SomeCondition);

// Modify each entity
foreach (var entity in entitiesToUpdate)
{
    entity.Name = "Updated Name";
}

// Asynchronously update the entities in the repository
var updatedEntities = await _countryRepository.UpdateRangeAsync(entitiesToUpdate);

// Save changes asynchronously to the database
await _countryRepository.SaveChangesAsync();
```


## Remove
Removes or Delete data from database

#### Remove
```csharp
// Retrieve the entity to remove
var entityToRemove = _countryRepository.Entities.GetById(entityId);

// Remove the entity from the context
_countryRepository.Remove(entityToRemove);

// Save changes to persist the removal to the database
 _countryRepository.SaveChanges();
```

#### RemoveAsync
Asynchronously removes a single entity from the database context.
```csharp
// Retrieve the entity to remove
var entityToRemove = await _countryRepository.Entities.GetByIdAsync(entityId);

// Asynchronously remove the entity from the context
await _countryRepository.RemoveAsync(entityToRemove);

// Save changes to persist the removal to the database
await _countryRepository.SaveChangesAsync();

```
#### RemoveRange
Removes a range of entities from the database context.
```csharp
// Retrieve the entities to remove
var entitiesToRemove = _countryRepository.Entities.Where(e => e.SomeCondition).ToList();

// Remove the entities from the context
_countryRepository.RemoveRange(entitiesToRemove);

// Save changes to persist the removal to the database
 _countryRepository.SaveChanges();
```
#### RemoveRangeAsync
```csharp
// Retrieve the entities to remove
var entitiesToRemove = await _countryRepository.Entities.FindAsync(e => e.SomeCondition).ToList();

// Asynchronously remove the entities from the context
await _countryRepository.RemoveRangeAsync(entitiesToRemove);

// Save changes to persist the removal to the database
await _countryRepository.SaveChangesAsync();
```

## Count 

#### Count()
Synchronously counts the total number of records of type T in the database.
```csharp
// Count all entities in the repository
int totalEntities = _countryRepository.Count();
Console.WriteLine($"Total entities: {totalEntities}");
```

#### Count() with with filtering
Synchronously counts the number of records that match a given condition (predicate).
```csharp
// Count entities where the country name starts with 'A'
int count = _countryRepository.Count(c => c.Name.StartsWith("A"));
Console.WriteLine($"Countries starting with 'A': {count}");
```

#### CountAsync()
Asynchronously counts the total number of records of type T in the database.
```csharp
// Asynchronously count all entities in the repository
int totalEntities = await _countryRepository.CountAsync();
Console.WriteLine($"Total entities: {totalEntities}");
```

#### CountAsync() with with filtering
Asynchronously counts the number of records that match a given condition (predicate).
```csharp
// Asynchronously count entities where the country name starts with 'A'
int count = await _countryRepository.CountAsync(c => c.Name.StartsWith("A"));
Console.WriteLine($"Countries starting with 'A': {count}");
```

#### CountLong()
Synchronously counts the total number of records of type T in the database, returning a long result for large datasets.
```csharp
// Count all entities with a long result type
long totalEntities = _countryRepository.CountLong();
Console.WriteLine($"Total entities (long): {totalEntities}");
```

#### CountLong() with with filtering
Synchronously counts the number of records that match a given condition (predicate), returning a long result for large datasets.
```csharp
// Count entities where the country name starts with 'A' and return as long
long count = _countryRepository.CountLong(c => c.Name.StartsWith("A"));
Console.WriteLine($"Countries starting with 'A' (long): {count}");
```

#### CountLongAsync()
Asynchronously counts the total number of records of type T in the database, returning a long result for large datasets.
```csharp
// Asynchronously count all entities with a long result type
long totalEntities = await _countryRepository.CountLongAsync();
Console.WriteLine($"Total entities (long): {totalEntities}");
```

#### CountLongAsync() with filtering
Asynchronously counts the number of records that match a given condition (predicate), returning a long result for large datasets.
```csharp
// Asynchronously count entities where the country name starts with 'A' and return as long
long count = await _countryRepository.CountLongAsync(c => c.Name.StartsWith("A"));
Console.WriteLine($"Countries starting with 'A' (long): {count}");
```

## Any
#### Any()
Synchronously checks whether any entities of type T exist in the database.
```csharp
// Check if there are any entities in the repository
bool hasEntities = _countryRepository.Any();
Console.WriteLine($"Any entities in the repository: {hasEntities}");
```

#### Any() with filtering
Synchronously checks whether any entities that match the specified condition (predicate) exist in the database.
```csharp
// Check if there are any entities where the country name starts with 'A'
bool hasEntities = _countryRepository.Any(c => c.Name.StartsWith("A"));
Console.WriteLine($"Any countries starting with 'A': {hasEntities}");
```

#### AnyAsync()
Asynchronously checks whether any entities of type T exist in the database.
```csharp
// Asynchronously check if there are any entities in the repository
bool hasEntities = await _countryRepository.AnyAsync();
Console.WriteLine($"Any entities in the repository: {hasEntities}");
```

#### AnyAsync() with filtering
Asynchronously checks whether any entities that match the specified condition (predicate) exist in the database.
```csharp
// Asynchronously check if there are any entities where the country name starts with 'A'
bool hasEntities = await _countryRepository.AnyAsync(c => c.Name.StartsWith("A"));
Console.WriteLine($"Any countries starting with 'A': {hasEntities}");
```

#### Transaction Management
Starts a new database transaction synchronously. This is useful when you want to execute multiple database operations as a single atomic operation.
```csharp
public void ExecuteWithTransaction()
{
    using (var transaction = _countryRepository.BeginTransaction())
    {
        try
        {
            // Perform multiple database operations
            _countryRepository.Add(new Country { Name = "Nepal" });
            _countryRepository.SaveChanges();

            _countryRepository.Add(new Country { Name = "UK" });
            _countryRepository.SaveChanges();

            // Commit the transaction
            transaction.Commit();
        }
        catch (Exception ex)
        {
            // Rollback the transaction if any operation fails
            transaction.Rollback();
            throw; // Re-throw the exception
        }
    }
}
```
