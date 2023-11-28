using System.Linq.Expressions;
using System.Reflection;
using Z.EntityFramework.Plus;
namespace Ndknitor.EFCore;
public static class QueryableExtension
{
    /// <summary>
    /// Method allows you to perform pagination on an IQueryable<T> by specifying the desired page number and page size.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    /// <summary>
    /// Method allows you to perform pagination on an IQueryable<T> while also getting the total number of records in the original query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    /// <summary>
    /// Method allows you to perform pagination on an IQueryable<T> while also deferring the total count calculation until a later time, potentially reducing the number of database round trips.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    /// <summary>
    /// Method allows you to perform sorting and ordering on an IQueryable<T> by specifying one or more columns to order by and whether each column should be in descending order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="orderBy"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
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
    public static IQueryable<TEntity> SelectFields<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeColumns)
    where TEntity : class
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var newExpression = Expression.New(typeof(TEntity));

        // Create a list of property assignments to include only the included columns
        var assignments = typeof(TEntity)
            .GetProperties()
            .Where(property => includeColumns.Any(expr => IsPropertyMatch(expr, property)))
            .Select(property => Expression.Bind(property, Expression.Property(parameter, property)))
            .ToList();

        var memberInitExpression = Expression.MemberInit(newExpression, assignments);
        var lambda = Expression.Lambda<Func<TEntity, TEntity>>(memberInitExpression, parameter);

        // Use the custom projection in the query
        return query.Select(lambda);
    }

    public static IQueryable<TEntity> SelectExcept<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] excludeColumns)
        where TEntity : class
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var newExpression = Expression.New(typeof(TEntity));

        // Create a list of property assignments to include all columns except the excluded ones
        var assignments = typeof(TEntity)
            .GetProperties()
            .Where(property => !excludeColumns.Any(expr => IsPropertyMatch(expr, property)))
            .Select(property => Expression.Bind(property, Expression.Property(parameter, property)))
            .ToList();

        var memberInitExpression = Expression.MemberInit(newExpression, assignments);
        var lambda = Expression.Lambda<Func<TEntity, TEntity>>(memberInitExpression, parameter);

        // Use the custom projection in the query
        return query.Select(lambda);
    }
    private static bool IsPropertyMatch<TEntity>(Expression<Func<TEntity, object>> expression, PropertyInfo property)
    {
        var memberInitExpression = expression.Body as NewExpression;
        if (memberInitExpression != null && memberInitExpression.Members.Count > 0)
        {
            return memberInitExpression.Members
                .Select(member => member.Name)
                .Contains(property.Name);
        }

        if (expression.Body is UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name == property.Name;
            }
        }
        else if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name == property.Name;
        }

        return false;
    }
    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}