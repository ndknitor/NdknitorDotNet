This class library mainly write for request validation in ASP.NET Core and some extension method for DbContext in EF Core
---
**Command to download package:**

```bash
dotnet add package Ndknitor
---
# Ndknitor.Web.Validations Namespace

- **AllowedExtensionsAttribute**: Validates file extensions for `IFormFile` uploads.
- **ClassPropertyAttribute**: Validates whether property names are valid for a specified target type.
- **FutureDateAttribute**: Ensures that a `DateTime` value is set to a date and time in the future.
- **FutureDateTimeAttribute**: Ensures that a `DateTime` value is set to a date and time in the future, including the current date and time.
- **GreaterThan**: Validates that a property's value is greater than another property's value.
- **ImageFileAttribute**: Validates that an uploaded file is an image based on its content type.
- **LeastOnePropertyAttribute**: Ensures that at least one of the specified properties has a non-null or non-empty value.
- **LessThan**: Validates that a property's value is less than another property's value.
- **MaxFileSizeAttribute**: Validates the maximum file size for `IFormFile` uploads.
- **OnlyOnePropertyAttribute**: Ensures that only one of the specified properties has a value.
- **PastDateAttribute**: Ensures that a `DateTime` value is set to a date and time in the past.
- **PastDateTimeAttribute**: Ensures that a `DateTime` value is set to a date and time in the past, including the current date and time.
- **SquareImageAttribute**: Validates that an uploaded image has a square aspect ratio.

---

# Ndknitor.System Namespace

The `Ndknitor.System` namespace contains utility and extension methods that provide additional functionality for working with collections and expressions.

## `MapProperty` Extension Method

### Description
The `MapProperty` extension method allows you to map a property of elements in one collection to another collection of values with the same length. This can be helpful when you need to update or synchronize properties between two collections.

### Signature
```csharp
public static IEnumerable<TSource> MapProperty<TSource, TResult>(
    this IEnumerable<TSource> targetArray,
    Expression<Func<TSource, TResult>> propertyExpression,
    IEnumerable<TResult> sourceArray)
```

- `targetArray` (IEnumerable<TSource>): The target collection whose property you want to map.
- `propertyExpression` (Expression<Func<TSource, TResult>>): A lambda expression specifying the property to be mapped.
- `sourceArray` (IEnumerable<TResult>): The source collection containing the values to map to the target collection.

### Exceptions
- `ArgumentNullException`: If any of the input parameters (`targetArray`, `propertyExpression`, `sourceArray`) is `null`.
- `ArgumentException`: If the `targetArray` and `sourceArray` have different lengths.

### Behavior
- The method maps the specified property of elements in the `targetArray` to corresponding values in the `sourceArray`.
- It iterates through both collections and updates the property value in the `targetArray` with the corresponding value from the `sourceArray`.

**Returns:** The `targetArray` with the updated property values.

### Example Usage

```csharp
var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Alice" },
    new Employee { Id = 2, Name = "Bob" },
};

var salaries = new List<decimal> { 50000, 60000 };

employees.MapProperty(e => e.Salary, salaries);

// The 'Salary' property of 'employees' will be updated with values from 'salaries'.
```
In this example, the `MapProperty` extension method is used to map the `Salary` property of `Employee` objects in the `employees` collection to values from the `salaries` collection.

## `ToClass<T>` Extension Method

### Description
The `ToClass<T>` extension method allows you to deserialize a JSON string into an object of a specified type `T`. It is a convenient way to convert JSON data into a strongly-typed object.

### Signature
```csharp
public static T ToClass<T>(this string str)
```

- `str` (string): The JSON string to be deserialized.
- Returns an object of type `T` after deserializing the JSON string.

### Dependencies
This method relies on the `JsonConvert` class from the Newtonsoft.Json library for JSON deserialization. Ensure that you have this library referenced in your project for the method to work.

### Example Usage

```csharp
string json = "{\"Name\":\"John\",\"Age\":30,\"City\":\"New York\"}";
Person person = json.ToClass<Person>();
```
In this example, the `ToClass<T>` extension method is used to deserialize a JSON string `json` into a `Person` object.

## `ToJson` Extension Method

### Description
The `ToJson` extension method is used to serialize an object to its JSON representation. It provides a convenient way to convert an object into a JSON string, which can be useful for data exchange, storage, or communication with other systems.

### Signature
```csharp
public static string ToJson(this object obj)
```

- `obj` (object): The object to be serialized to JSON.

**Returns:** A JSON string representation of the object.

## `UserId<T>` Extension Method

### Description
The `UserId<T>` extension method extends the functionality of the `ClaimsPrincipal` class. It is used to extract a specific user identifier from the claims associated with a `ClaimsPrincipal` object. The user identifier is often used to uniquely identify a user in authentication and authorization scenarios.

### Signature
```csharp
public static T UserId<T>(this ClaimsPrincipal principal, string key = ClaimTypes.NameIdentifier)
```

- `principal` (ClaimsPrincipal): The `ClaimsPrincipal` object from which to extract the user identifier.
- `key` (string, optional): The claim type to use when searching for the user identifier. Defaults to `ClaimTypes.NameIdentifier`.

**Returns:** The user identifier of type `T` if found in the claims, or the default value of type `T` if not found.

---

# Ndknitor.EFCore Namespace

The `Ndknitor.EFCore` namespace contains extension methods that provide functionality related to auto-increment primary keys in Entity Framework Core applications.

## `GetLastInsertedId<TEntity, TKey>` Extension Method

### Description
The `GetLastInsertedId<TEntity, TKey>` extension method is used to retrieve the last inserted auto-increment primary key value of a specific entity type `TEntity` from the database context. This method can be helpful when you need to obtain the ID of a newly inserted entity.

### Signature
```csharp
public static TKey GetLastInsertedId<TEntity, TKey>(this DbContext context)
```

- `context` (DbContext): The database context from which to retrieve the last inserted ID.

**Returns:** The last inserted auto-increment primary key value of type `TKey` for the specified entity type.

## `GetInsertId<TEntity, TKey>` Extension Method

### Description
The `GetInsertId<TEntity, TKey>` extension method is used to retrieve the auto-increment primary key value of a newly inserted entity of type `TEntity` from the database context. This method can be used to obtain the ID of an entity immediately after inserting it.

### Signature
```csharp
public static TKey GetInsertId<TEntity, TKey>(this DbContext context)
```

- `context` (DbContext): The database context from which to retrieve the last inserted ID.

**Returns:** The auto-increment primary key value of type `TKey` for the newly inserted entity of type `TEntity`.

## `GetInsertEntity<TEntity, TKey>` Extension Method

### Description
The `GetInsertEntity<TEntity, TKey>` extension method is used to retrieve the newly inserted entity of type `TEntity` from the database context after insertion and attach its auto-increment primary key value of type `TKey` to the entity parameter. This method is useful when you need to work with the entire entity immediately after insertion and want to have access to the generated primary key value.

### Signature
```csharp
public static TEntity GetInsertEntity<TEntity, TKey>(this DbContext context, TEntity entity)
```

- `context` (DbContext): The database context where the entity was inserted.
- `entity` (TEntity): The entity that was inserted.

**Returns:** The newly inserted entity of type `TEntity` along with its auto-increment primary key value of type `TKey`.

## `GetInsertEntities<TEntity, TKey>` Extension Method

### Description
The `GetInsertEntities<TEntity, TKey>` extension method is used to retrieve a collection of newly inserted entities of type `TEntity` from the database context after insertion. For each entity in the collection, it attaches its auto-increment primary key value of type `TKey` to the entity parameter. This method is helpful when you need to work with multiple inserted entities immediately after insertion and want to have access to the generated primary key values for each entity.

### Signature
```csharp
public static IEnumerable<TEntity> GetInsertEntities<TEntity, TKey>(this DbContext context, IEnumerable<TEntity> entities)
```

- `context` (DbContext): The database context where the entities were inserted.
- `entities` (IEnumerable<TEntity>): The collection of entities that were inserted.

**Returns:** A collection of newly inserted entities of type `TEntity` along with their auto-increment primary key values of type `TKey`.


## `Paginate<T>` Extension Method

### Description
The `Paginate<T>` extension method allows you to perform pagination on an `IQueryable<T>` by specifying the desired page number and page size. It retrieves a specific page of results from the queryable data.

### Signature
```csharp
public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int page, int pageSize = int.MaxValue)
```

- `queryable` (IQueryable<T>): The IQueryable to be paginated.
- `page` (int): The page number to retrieve.
- `pageSize` (int, optional): The number of items per page. Defaults to `int.MaxValue`.

**Returns:** An IQueryable containing the specified page of results.

## `Paginate<T>` Extension Method with Total Count

### Description
The `Paginate<T>` extension method with a total count out parameter allows you to perform pagination on an `IQueryable<T>` while also getting the total number of records in the original query.

### Signature
```csharp
public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int page, int pageSize, out int total)
```

- `queryable` (IQueryable<T>): The IQueryable to be paginated.
- `page` (int): The page number to retrieve.
- `pageSize` (int): The number of items per page.
- `total` (out int): An out parameter that receives the total number of records in the original query.

**Returns:** An IQueryable containing the specified page of results.

## `DeferredPaginate<T>` Extension Method with Deferred Total Count

### Description
The `DeferredPaginate<T>` extension method with a deferred total count out parameter allows you to perform pagination on an `IQueryable<T>` while also deferring the total count calculation until a later time, potentially reducing the number of database round trips.

### Signature
```csharp
public static IQueryable<T> DeferredPaginate<T>(this IQueryable<T> queryable, int page, int pageSize, out QueryFutureValue<int> total)
```

- `queryable` (IQueryable<T>): The IQueryable to be paginated.
- `page` (int): The page number to retrieve.
- `pageSize` (int): The number of items per page.
- `total` (out QueryFutureValue<int>): An out parameter that provides a deferred total count value, allowing the count calculation to be deferred.

**Returns:** An IQueryable containing the specified page of results.

**Note:** When using the `DeferredPaginate` method, the total count calculation and retrieval will be deferred until explicitly requested, potentially optimizing database round trips.

**Example Usage**

```csharp
var query = dbContext.SomeEntities.AsQueryable();
var page = 1;
var pageSize = 10;
IQueryable<SomeEntity> pagedQuery = query.Paginate(page, pageSize);
```

In this example, the `Paginate` extension method is used to retrieve the first page of results from the `SomeEntities` DbSet with a specified page number and page size.

## `OrderBy<T>` Extension Method

### Description
The `OrderBy<T>` extension method allows you to perform sorting and ordering on an `IQueryable<T>` by specifying one or more columns to order by and whether each column should be in descending order. It provides a flexible way to order query results dynamically based on multiple columns.

### Signature
```csharp
public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<string> orderBy, IEnumerable<bool> desc)
```

- `source` (IQueryable<T>): The IQueryable to be sorted.
- `orderBy` (IEnumerable<string>): A collection of column names to order by.
- `desc` (IEnumerable<bool>): A collection of boolean values indicating whether each corresponding column should be ordered in descending order.

**Returns:** An IOrderedQueryable<T> that represents the sorted query.

### Behavior
- The method allows you to specify multiple columns to order by, and for each column, whether it should be ordered in ascending or descending order.
- It supports dynamic sorting based on the provided column names and sort directions.

**Example Usage**

```csharp
var query = dbContext.SomeEntities.AsQueryable();
var orderByColumns = new List<string> { "ColumnName1", "ColumnName2" };
var isDescending = new List<bool> { false, true };

IOrderedQueryable<SomeEntity> sortedQuery = query.OrderBy(orderByColumns, isDescending);
```

In this example, the `OrderBy` extension method is used to order the `SomeEntities` DbSet query based on two columns, `"ColumnName1"` in ascending order and `"ColumnName2"` in descending order.

---