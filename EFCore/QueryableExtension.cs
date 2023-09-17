using System.Linq.Expressions;
using Z.EntityFramework.Plus;
namespace Ndknitor.System.EFCore;
public static class QueryableExtension
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int page, int pageSize = int.MaxValue)
    {
        if (page < 1)
        {
            return queryable;
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");
        }

        int skip = (page - 1) * pageSize;

        return queryable.Skip(skip).Take(pageSize);
    }
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int page, int pageSize, out int total)
    {
        total = 0;
        if (page < 1)
        {
            return queryable;
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");
        }

        int skip = (page - 1) * pageSize;
        total = queryable.Count();
        return queryable.Skip(skip).Take(pageSize);
    }
    public static IQueryable<T> DeferredPaginate<T>(this IQueryable<T> queryable, int page, int pageSize, out QueryFutureValue<int> total)
    {
        total = null;
        if (page < 1)
        {
            return queryable;
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");
        }

        int skip = (page - 1) * pageSize;
        total = queryable.DeferredCount().FutureValue();
        return queryable.Skip(skip).Take(pageSize);
    }
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool isDescending)
    {
        var lambdaExpression = ToLambda<T>(propertyName);

        if (isDescending)
        {
            return source.OrderByDescending(lambdaExpression);
        }
        else
        {
            return source.OrderBy(lambdaExpression);
        }
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<string> orderBy, IEnumerable<bool> desc)
    {
        IOrderedQueryable<T> orderedQuery = null;

        if (orderBy == null || !orderBy.Any())
        {
            var isDescending = desc != null && desc.Count() > 0 ? desc.ElementAt(0) : false;
            if (isDescending)
            {
                return source.OrderByDescending(e => e);
            }
            else
            {
                return source.OrderBy(e => e);
            }
        }

        if (desc == null)
        {
            // If desc is not provided, default to all false values.
            desc = Enumerable.Repeat(false, orderBy.Count());
        }
        else if (orderBy.Count() != desc.Count())
        {
            // If desc is provided but shorter than orderBy, pad it with false values.
            var missingCount = orderBy.Count() - desc.Count();
            desc = desc.Concat(Enumerable.Repeat(false, missingCount)).ToList();
        }

        for (int i = 0; i < orderBy.Count(); i++)
        {
            var propertyName = orderBy.ElementAt(i);
            var isDescending = desc.ElementAt(i);
            var lambdaExpression = ToLambda<T>(propertyName);

            if (i == 0)
            {
                orderedQuery = isDescending
                    ? source.OrderByDescending(lambdaExpression)
                    : source.OrderBy(lambdaExpression);
            }
            else
            {
                orderedQuery = isDescending
                    ? orderedQuery.ThenByDescending(lambdaExpression)
                    : orderedQuery.ThenBy(lambdaExpression);
            }
        }

        return orderedQuery;
    }

    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}