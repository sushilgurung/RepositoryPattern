using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Interfaces.Services
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Queryable { get; }
        IQueryable<T> GetQueryable();

        #region Get
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllAsNoTracking();

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsNoTrackingAsync();

        #endregion

        #region GetById
        T GetById(int id);
        T GetById(long id);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(long id);
        #endregion

        #region First or Default
        T FirstOrDefault();
        Task<T> FirstOrDefaultAsync();

        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        T FirstOrDefault(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<T> FirstOrDefaultAsync(params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);


        T FirstOrDefault(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys);

        #endregion



        #region Add
        T Add(T entity);
        Task<T> AddAsync(T entity);
        #endregion

        #region Update
        T Update(T entity);
        Task UpdateAsync(T entity);
        #endregion

        #region Save Changes
        int SaveChanges();
        Task<int> SaveChangesAsync();
        #endregion
    }
}
