# Repository Pattern with Entity Framework Core

## Overview

The Repository Pattern provides a way to encapsulate data access logic and abstract away the details of data storage and retrieval. This approach promotes separation of concerns, making your application more maintainable and testable.

This repository pattern implementation leverages Entity Framework Core (EF Core) for data access. It includes a generic repository interface that defines a comprehensive set of methods for querying, adding, updating, and deleting entities.

### Target Framework
This project is built with .NET 5.0 and uses Entity Framework Core version 5.0.17.

## Repository Interface

The `IRepository<T>` interface offers a wide range of methods for interacting with your data. Below is a summary of the available methods:

### Queryable

- `IQueryable<T> Queryable { get; }`
- `IQueryable<T> GetQueryable()`

### Get

- `IEnumerable<T> GetAll()`
- `IEnumerable<T> GetAllAsNoTracking()`
- `Task<IEnumerable<T>> GetAllAsync()`
- `Task<IEnumerable<T>> GetAllAsNoTrackingAsync()`

### GetById

- `T GetById(int id)`
- `T GetById(long id)`
- `Task<T> GetByIdAsync(int id)`
- `Task<T> GetByIdAsync(long id)`

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
- `Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)`
- `IEnumerable<T> FindAsNoTracking(Expression<Func<T, bool>> predicate)`
- `Task<IEnumerable<T>> FindAsyncAsNoTracking(Expression<Func<T, bool>> predicate)`

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

## Example Usage

### Defining an Entity

## Configuring Services in Startup

To add the repository service to the dependency injection container, use the `RepositoryServiceRegistration.ConfigureServices` method in the `Startup` class. Here's how you can do it:

### Example

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

        // Other service registrations
    }

    // Other methods
}
```
## Using `FirstOrDefaultAsync` with predicate and order

The `FirstOrDefaultAsync` method allows you to retrieve the first record that matches specified filtering criteria and sorting order. It accepts a filtering predicate and a series of sorting criteria, which can be used to order the results.

### Method Signature

```csharp
public async Task<MyEntity> GetTopActiveEntityAsync()
{
    // Call the FirstOrDefaultAsync method with filtering and sorting criteria directly
    var topEntity = await _repository.FirstOrDefaultAsync(
        x => x.IsActive && x.Population > 1000000, // Filtering predicate
        (x => x.Name, false),       // Sort by Name ascending
        (x => x.Population, true),   // Then by Population descending
        (x => x.Area, false)         // Finally by Area ascending
    );

    return topEntity;
}

```
