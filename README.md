[![NuGet](https://img.shields.io/nuget/v/ndknitor.svg)](https://www.nuget.org/packages/Ndknitor)
[![Unit test for version 6](https://github.com/ndknitor/NdknitorDotNet/actions/workflows/dotnet6.yml/badge.svg)](https://github.com/ndknitor/NdknitorDotNet/actions/workflows/dotnet6.yml)
[![Unit test for version 8](https://github.com/ndknitor/NdknitorDotNet/actions/workflows/dotnet8.yml/badge.svg)](https://github.com/ndknitor/NdknitorDotNet/actions/workflows/dotnet8.yml)
---

This class library mainly write for request validation in ASP.NET Core and some extension method for DbContext in EF Core
---

**Command to download package:**

```bash
dotnet add package Ndknitor
```

---

# Ndknitor.Web.Validations Namespace

- **AllowedExtensionsAttribute**: Validates file extensions for `IFormFile` uploads.
- **ClassPropertyAttribute**: Validates whether property names are valid for a specified target type.
- **FutureDateAttribute**: Ensures that a `DateTime` value is set to a date and time in the future.
- **FutureDateTimeAttribute**: Ensures that a `DateTime` value is set to a date and time in the future, including the current date and time.
- **GreaterThanAttribute**: Validates that a property's value is greater than another property's value.
- **ImageFileAttribute**: Validates that an uploaded file is an image based on its content type.
- **LeastOnePropertyAttribute**: Ensures that at least one of the specified properties has a non-null or non-empty value.
- **LessThanAttribute**: Validates that a property's value is less than another property's value.
- **MaxFileSizeAttribute**: Validates the maximum file size for `IFormFile` uploads.
- **OnlyOnePropertyAttribute**: Ensures that only one of the specified properties has a value.
- **PastDateAttribute**: Ensures that a `DateTime` value is set to a date and time in the past.
- **PastDateTimeAttribute**: Ensures that a `DateTime` value is set to a date and time in the past, including the current date and time.
- **SquareImageAttribute**: Validates that an uploaded image has a square aspect ratio.
- **RegularTextAttribute**: Validates whether property values are valid based on a specified regular expression pattern.

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

## `MapIncreasement` Extension Method

### Description

The `MapIncreasement` extension method allows you to map a property of elements in one collection to a start number. This can be helpful when you need to have application level auto-increment key.

### Signature

```csharp
public static IEnumerable<T> MapIncreasement<T>(this IEnumerable<T> array, Action<T, int> expression, int offset)
```

- `array` (IEnumerable<T>): The target collection whose property you want to map.
- `expression` (Action<T, int>): A lambda expression specifying the property to be mapped.
- `offset` (int): The source collection containing the values to map to the target collection.

### Behavior

- The method maps the specified property of elements in the `array` to number start in `offset` + 1.
- It iterates through both collections and updates the property value in the `targetArray` with the corresponding value from the `sourceArray`.

### Example Usage

```csharp
seats.MapIncreasement((s, o) => s.SeatId = o, context.Seat.Max(s => s.SeatId));
// The 'SeatId' property of 'seats' will be maped incrementaly start from context.Seat.Max(s => s.SeatId) + 1.
```

# ToJsonClass Extension Method

## Description

The `ToJsonClass` extension method is designed to deserialize a JSON string into an instance of a specified class (`T`). This method uses the `JsonConvert` class from the JSON.NET library to perform the deserialization.

## Method Signature

```csharp
public static T ToJsonClass<T>(this string str)
```

### Parameters

- `str`: The JSON string to be deserialized into an instance of the specified class.

### Type Parameters

- `T`: The type of the class to which the JSON string should be deserialized.

### Return Type

- `T`: An instance of the specified class (`T`) representing the deserialized JSON data.

## Usage

```csharp
// Assuming a JSON string
string jsonString = GetJsonString();

// Convert the JSON string to an instance of MyClass
MyClass myObject = jsonString.ToJsonClass<MyClass>();
```

## `ToJson` Extension Method

### Description

The `ToJson` extension method is used to serialize an object to its JSON representation. It provides a convenient way to convert an object into a JSON string, which can be useful for data exchange, storage, or communication with other systems.

### Signature

```csharp
public static string ToJson(this object obj)
```

- `obj` (object): The object to be serialized to JSON.

**Returns:** A JSON string representation of the object.

# ToBson Extension Method

## Description

The `ToBson` extension method is designed to convert an object into its BSON (Binary JSON) representation. BSON is a binary representation of JSON-like documents, commonly used for data storage and transmission in MongoDB.

## Method Signature

```csharp
public static byte[] ToBson(this object value)
```

### Parameters

- `value`: The object to be serialized into BSON.

### Return Type

- `byte[]`: The BSON representation of the input object as a byte array.

## Implementation

The method utilizes the `MemoryStream` and `BsonDataWriter` classes to perform the BSON serialization. The `JsonSerializer` from the JSON.NET library is used for the serialization process.

# ToBsonClass Extension Method

## Description

The `ToBsonClass` extension method is designed to convert a byte array containing BSON (Binary JSON) data into an instance of a specified class (`T`). BSON is a binary representation of JSON-like documents, commonly used for data storage and transmission in MongoDB.

## Method Signature

```csharp
public static T ToBsonClass<T>(this byte[] data)
```

### Parameters

- `data`: The byte array containing BSON data to be deserialized.

### Type Parameters

- `T`: The type of the class to which the BSON data should be deserialized.

### Return Type

- `T`: An instance of the specified class (`T`) representing the deserialized BSON data.

## Usage

```csharp
// Assuming a byte array containing BSON data
byte[] bsonData = GetBsonData();

// Convert the BSON data to an instance of MyClass
MyClass myObject = bsonData.ToBsonClass<MyClass>();
```

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

# Ndknitor.Services.Web Namespace

The `Ndknitor.Services.Web` namespace contains some pre-defined services and helper for Asp.Net to make it easily to scalable.

## `KeyBasedCookieDataFormat` Class

The `KeyBasedCookieDataFormat` class is a custom implementation of the `ISecureDataFormat<AuthenticationTicket>` interface in ASP.NET Core. It is designed to provide cookie authentication with the ability to use a customizable encryption key. This class is especially friendly with horizontal scaling as it allows you to control the key used for encrypting and decrypting authentication tickets.

## Usage

### Initialization

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.TicketDataFormat = new KeyBasedCookieDataFormat("your-super-secret-key");
});
```

### Key Features

It have flexibility in handling the encryption key. By allowing you to provide a custom key during initialization, it becomes highly suitable for scenarios involving horizontal scaling. Different instances of your application can share the same authentication key, ensuring seamless authentication across the scaled-out instances.

### Description

The capability is particularly useful in distributed environments, such as microservices architectures, where multiple instances of the application may need to authenticate users independently while sharing the same user authentication information.

Remember to keep your authentication key secure and do not expose it to unauthorized personnel.

# Ndknitor.EFCore Namespace

The `Ndknitor.EFCore` namespace contains extension methods that provide flexible way to select data in Entity Framework Core applications.

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
var page = 1;
var pageSize = 10;
QueryFutureValue<int> total = null;
IQueryable<Seat> = context.Seat
    .Where(s => s.Deleted == false)
    .DeferredPaginate(page, pageSize, out total);
```

In this example, the `Paginate` extension method is used to retrieve the first page of results from the `Seat` DbSet with a specified page number and page size.

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
