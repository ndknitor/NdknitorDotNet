using System.Linq.Expressions;
using System.Reflection;
using Z.EntityFramework.Plus;
namespace Ndknitor.EFCore;
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

    public static IQueryable<T> CursorPaginate<T, TKey>(this IQueryable<T> queryable, TKey cursor, int pageSize = int.MaxValue) where T : class
    {
        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");
        }

        if (cursor == null)
        {
            return queryable.Take(pageSize);
        }

        var keyProperty = typeof(T).GetProperties().FirstOrDefault(p => p.PropertyType == typeof(TKey));

        if (keyProperty == null)
        {
            throw new InvalidOperationException("The entity does not have a property of type TKey.");
        }

        return queryable
            .Where(BuildCursorPredicate<T, TKey>(cursor, keyProperty))
            .Take(pageSize);
    }
    public static IQueryable<T> CursorPaginate<T, TKey>(this IQueryable<T> queryable, TKey cursor, int pageSize, out int total) where T : class
    {
        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");
        }
        total = queryable.Count();
        if (cursor == null)
        {
            return queryable.Take(pageSize);
        }

        var keyProperty = typeof(T).GetProperties().FirstOrDefault(p => p.PropertyType == typeof(TKey));

        if (keyProperty == null)
        {
            throw new InvalidOperationException("The entity does not have a property of type TKey.");
        }

        return queryable
            .Where(BuildCursorPredicate<T, TKey>(cursor, keyProperty))
            .Take(pageSize);
    }
    public static IQueryable<T> DeferredCursorPaginate<T, TKey>(this IQueryable<T> queryable, TKey cursor, int pageSize, out QueryFutureValue<int> total) where T : class
    {
        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");
        }

        var keyProperty = typeof(T).GetProperties().FirstOrDefault(p => p.PropertyType == typeof(TKey));

        if (keyProperty == null)
        {
            throw new InvalidOperationException("The entity does not have a property of type TKey.");
        }

        total = queryable.DeferredCount().FutureValue();

        if (cursor == null)
        {
            return queryable.Take(pageSize);
        }

        var predicate = BuildCursorPredicate<T, TKey>(cursor, keyProperty);

        return queryable
            .Where(predicate)
            .Take(pageSize);
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
    private static Expression<Func<T, bool>> BuildCursorPredicate<T, TKey>(TKey cursor, PropertyInfo keyProperty)
    {
        if (!IsNumericType(typeof(TKey)))
        {
            throw new InvalidOperationException("Cursor pagination does not support non-number primary key.");
        }
        var parameter = Expression.Parameter(typeof(T));
        var propertyAccess = Expression.Property(parameter, keyProperty);
        var cursorValue = Expression.Constant(cursor, typeof(TKey));

        // Assuming TKey is a comparable type
        var predicateBody = Expression.GreaterThan(propertyAccess, cursorValue);

        return Expression.Lambda<Func<T, bool>>(predicateBody, parameter);
    }
    private static bool IsNumericType(Type type)
    {
        return type == typeof(byte) ||
               type == typeof(sbyte) ||
               type == typeof(short) ||
               type == typeof(ushort) ||
               type == typeof(int) ||
               type == typeof(uint) ||
               type == typeof(long) ||
               type == typeof(ulong) ||
               type == typeof(float) ||
               type == typeof(double) ||
               type == typeof(decimal);
    }
}