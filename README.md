# Become a Patron
**If you want to encourage me to contribute to this repository more frequently. Then please become a patron on here: https://www.patreon.com/user?u=80125977**

Thanks in advance.

# EF Core Generic Repository

This library is a Generic Repository implementation for EF Core ORM which will remove developers' pain to write repository layer for each .NET Core and .NET project.

## ⭐ Giving a star

**If you find this library useful, please don't forget to encouraging me to do such more stuffs by giving a star to this repository. Thank you.**

## 🔥 What's new

### Pagination Support:
```C#
PaginationSpecification<Employee> specification = new PaginationSpecification<Employee>();
specification.Conditions.Add(e => e.Name.Contains("Ta"));
specification.PageIndex = 1;
specification.PageSize = 10;

PaginatedList<EmployeeDto> paginatedList = await _repository.GetPaginatedListAsync(specification, e => new EmployeeDto
{
    Id = e.Id
    Name = e.Name,
    DepartmentName = e.DepartmentName
});
```

### Free raw SQL support:

```C#
List<string> search = new List<string>() { "Tanvir", "Software" };
string sqlQuery = "Select EmployeeName, DepartmentName from Employee Where EmployeeName LIKE @p0 + '%' and DepartmentName LIKE @p1 + '%'";
List<EmployeeDto> items = await _repository.GetFromRawSqlAsync<EmployeeDto>(sqlQuery, search);

```

## ⚙️ This library includes following notable features:

1. This library can be run on any .NET Core or .NET application which has .NET Core 3.1, .NET Standard 2.1 and .NET 5.0 support.

2. It’s providing the Generic Repository with database transaction support.

3. It has all the required methods to query your data in whatever way you want without getting IQueryable<T> from the repository.

4. It also has **`Specification<T>`** pattern support so that you can build your query dynamically i.e. differed query building.

5. It also has database level projection support for your query.

6. It also has support to run raw SQL command against your relational database.

7. It also has support to choose whether you would like to track your query entity/entities or not.

8. It also has support to reset your EF Core DbContext state whenever you really needed.

9. Most importantly, it has full Unit Testing support.

11. Pagination support.

13. Free raw SQL query support both for complex type and primitive types.

## ✈️ How do I get started?

### For full version (both query and command support):
    
First install the latest version of `TanvirArjel.EFCore.GenericRepository` [nuget](https://www.nuget.org/packages/TanvirArjel.EFCore.GenericRepository) package into your project as follows:

**Package Manager Console:**

```C#
Install-Package TanvirArjel.EFCore.GenericRepository
```
    
**.NET CLI:**

```C#
dotnet add package TanvirArjel.EFCore.GenericRepository
```
    
Then in the `ConfirugeServices` method of the `Startup` class:

```C#
public void ConfigureServices(IServiceCollection services)
{
    // For single DbContext
    services.AddGenericRepository<YourDbContext>();
    
    // If multiple DbContext
    services.AddGenericRepository<YourDbContext1>();
    services.AddGenericRepository<YourDbContext1>();
}
```

### For query version only:
    
First install the latest version of `TanvirArjel.EFCore.QueryRepository` [nuget](https://www.nuget.org/packages/TanvirArjel.EFCore.QueryRepository) package into your project as follows:

**Package Manager Console:**

```C#
Install-Package TanvirArjel.EFCore.QueryRepository
```
    
**.NET CLI:**

```C#
dotnet add package TanvirArjel.EFCore.QueryRepository
```
    
Then in the `ConfirugeServices` method of the `Startup` class:

```C#
public void ConfigureServices(IServiceCollection services)
{
    // For single DbContext
    services.AddQueryRepository<YourDbContext>();
    
    // For multiple DbContext
    services.AddQueryRepository<YourDbContext1>();
    services.AddQueryRepository<YourDbContext1>();
}
```
    
## 🛠️ Usage: Query

```C#
public class EmployeeService
{
    // For query version, please use `IQueryRepository` instead of `IRepository`
    private readonly IRepository _repository; // If single DbContext
    private readonly IRepository<YourDbContext1> _dbConext1Repository; // If multiple DbContext

    public EmployeeService(IRepository repository, IRepository<YourDbContext1> dbConext1Repository)
    {
        _repository = repository;
        _dbConext1Repository = dbConext1Repository;
    }

    public async Task<Employee> GetEmployeeAsync(int employeeId)
    {
        Employee employee = await _repository.GetByIdAsync<Employee>(1);
        return employee;
    }
}
```
## 🛠️ Usage: Command

```C#
public class EmployeeService
{
    private readonly IRepository _repository; // If single DbContext
    private readonly IRepository<YourDbContext1> _dbConext1Repository; // If multiple DbContext

    public EmployeeService(IRepository repository, IRepository<YourDbContext1> dbConext1Repository)
    {
        _repository = repository;
        _dbConext1Repository = dbConext1Repository;
    }

    // Single database operation.
    public async Task<int> CreateAsync(Employee employee)
    {
        object[] primaryKeys = await _repository.InsertAsync(employee);
        return (int)primaryKeys[0];
    }

    // Multiple database operations.
    public async Task<int>> CreateAsync(Employee employee)
    {
       IDbContextTransaction transaction = await _repository.BeginTransactionAsync(IsolationLevel.ReadCommitted);
       try
       {
           object[] primaryKeys = await _repository.InsertAsync(employee);

           long employeeId = (long)primaryKeys[0];
           EmployeeHistory employeeHistory = new EmployeeHistory()
           {
               EmployeeId = employeeId,
               DepartmentId = employee.DepartmentId,
               EmployeeName = employee.EmployeeName
           };

           await _repository.InsertAsync(employeeHistory);

           await transaction.CommitAsync();

           return employeeId;
       }
       catch (Exception)
       {
           await transaction.RollbackAsync();
           throw;
       }
    }
}
```
    
### For more detail documentaion, please visit [Documentation Wiki](https://github.com/TanvirArjel/EFCore.GenericRepository/wiki)
