using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gurung.RepositoryPattern.Helpers
{
    internal static class QueryHelper
    {
        internal static IQueryable<T> FilterAndOrderBy<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate = null, params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            if (predicate is not null)
                query = query.Where(predicate);

            if (orderBys is not null || orderBys.Any())
            {
                // Apply the first sorting criterion
                IOrderedQueryable<T> orderedQuery = orderBys[0].Descending
                    ? query.OrderByDescending(orderBys[0].KeySelector)
                    : query.OrderBy(orderBys[0].KeySelector);

                // Apply subsequent sorting criteria using ThenBy or ThenByDescending
                for (int i = 1; i < orderBys.Length; i++)
                {
                    var orderBy = orderBys[i];
                    orderedQuery = orderBy.Descending
                        ? orderedQuery.ThenByDescending(orderBy.KeySelector)
                        : orderedQuery.ThenBy(orderBy.KeySelector);
                }
                return orderedQuery;
            }
            return query;
        }

        internal static IQueryable<T> FilterAndOrderByAndSkip<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate = null,
            params (Expression<Func<T, object>> KeySelector, bool Descending)[] orderBys)
        {
            if (predicate is not null)
                query = query.Where(predicate);

            if (orderBys is not null || orderBys.Any())
            {
                // Apply the first sorting criterion
                IOrderedQueryable<T> orderedQuery = orderBys[0].Descending
                    ? query.OrderByDescending(orderBys[0].KeySelector)
                    : query.OrderBy(orderBys[0].KeySelector);

                // Apply subsequent sorting criteria using ThenBy or ThenByDescending
                for (int i = 1; i < orderBys.Length; i++)
                {
                    var orderBy = orderBys[i];
                    orderedQuery = orderBy.Descending
                        ? orderedQuery.ThenByDescending(orderBy.KeySelector)
                        : orderedQuery.ThenBy(orderBy.KeySelector);
                }
                return orderedQuery;
            }
            return query;
        }

    }
}
