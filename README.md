# EF Core Generic Repository

This library is Generic Repository implementation for EF Core ORM which will remove developers' pain to write repository layer for each .NET Core and .NET project.

## ⭐ Giving a star ⭐

**If you find this library useful, please don't forget to encouraging me to do such more stuffs by giving a star to this repository. Thank you.**

## 🔥 Breaking Changes in version 5.2.1 🔥

1. `repository.SaveChangesAsync()` method has been removed. Please look at the below usage documention for more details.
 

## ⚙️ This library includes following notable features: ⚙️

1. This library can be run on any .NET Core or .NET application which has .NET Core 3.1, .NET Standard 2.1 and .NET 5.0 support.

2. It’s providing the Generic Repository with database transaction support.

3. It has all the required methods to query your data in whatever way you want without getting IQueryable<T> from the repository.

4. It also has **`Specification<T>`** pattern support so that you can build your query dynamically i.e. differed query building.

5. It also has database level projection support for your query.

6. It also has support to run raw SQL command against your relational database.

7. It also has support to choose whether you would like to track your query entity/entities or not.

8. It also has support to reset your EF Core DbContext state whenever you really needed.

9. Most importantly, it has full Unit Testing support.

## ✈️ How do I get started? ✈️

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
    services.AddGenericRepository<YourDbContext>();
}
```
    
## 🛠️ Usage: Query 🛠️

```C#
public class EmployeeService
{
    private readonly IRepository _repository;

    public EmployeeService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Employee> GetEmployeeAsync(int employeeId)
    {
        Employee employee = await _repository.GetByIdAsync<Employee>(1);
        return employee;
    }
}
```
## 🛠️ Usage: Command 🛠️

```C#
public class EmployeeService
{
    private readonly IRepository _repository;

    public EmployeeService(IRepository repository)
    {
        _repository = repository;
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
    
## 🕮 More Details: 🕮

#### 1. To get all the data:

```C#
var employeeList =  await _repository.GetListAsync<Employee>();

var noTrackedEmployeeList = await _repository.GetListAsync<Employee>(asNoTracking: true);
````
    
#### 2. To get a filtered list of data:

```C#
var employeeList =  await _repository.GetListAsync<Employee>(e => e.EmployeeName.Contains("Tanvir") && e.DepartmentName == "Software");

var noTrackedEmployeeList = await _repository
                            .GetListAsync<Employee>(e => e.EmployeeName.Contains("Tanvir") && e.DepartmentName == "Software", asNoTracking: true);
```

#### 3. To get a list of data by Specification<T>:

```C#
Specification<Employee> specification = new Specification<Employee>();
specification.Conditions.Add(e => e.EmployeeName.Contains("Tanvir"));
specification.Includes = ep => ep.Include(e => e.Department);
specification.OrderBy = sp => sp.OrderBy(e => e.EmployeeName).ThenBy(e => e.DepartmentName);
specification.Skip = 0;
specification.Take = 10;

List<Employee> employeeList = await _repository.GetListAsync<Employee>(specification);

List<Employee> noTrackedEmployeeList = await _repository.GetListAsync<Employee>(specification, true);
```

 #### 4. To get the projected entity list:

```C#
Expression<Func<Employee, object>> selectExpression = e => new { e.EmployeeId, e.EmployeeName };
var projectedList = await _repository.GetProjectedListAsync<Employee, object>(selectExpression);
```

 #### 5. To get filtered projected entity list:

```C#
Expression<Func<Employee, object>> selectExpression = e => new { e.EmployeeId, e.EmployeeName };
var filteredProjectedList = await _repository.GetProjectedListAsync<Employee, object>(e => e.IsActive, selectExpression);
```

 #### 6. To get the projected entity list by `Specification<T>`:

```C#
Specification<Employee> specification = new Specification<Employee>();
specification.Conditions.Add(e => e.EmployeeName.Contains("Tanvir"));
specification.Includes = ep => ep.Include(e => e.Department);
specification.OrderBy = sp => sp.OrderBy(e => e.EmployeeName).ThenBy(e => e.DepartmentName);
specification.Skip = 0;
specification.Take = 10;

Expression<Func<Employee, object>> selectExpression = e => new { e.EmployeeId, e.EmployeeName };
var projectedList = await _repository.GetProjectedListAsync<Employee, object>(specification, selectExpression);
```

#### 7. To get an entity by Id (primary key):

```C#
Employee employee = await _repository.GetByIdAsync<Employee>(1);

Employee noTrackedEmployee = await _repository.GetByIdAsync<Employee>(1, true);
```

#### 8. To get a projected entity by Id (primary key):

```C#
Expression<Func<Employee, object>> selectExpression = e => new { e.EmployeeId, e.EmployeeName };
var projectedEntity = await _repository.GetProjectedByIdAsync<Employee, object>(1, selectExpression);
```

#### 9. To get a single entity by any condition/filter:

```C#
Employee employee = await _repository.GetAsync<Employee>(e => e.EmployeeName == "Tanvir");

Employee noTrackedEmployee = await _repository.GetAsync<Employee>(e => e.EmployeeName == "Tanvir", true);
```
    
#### 10. To get a single entity by `Specification<T>`:

```C#
Specification<Employee> specification = new Specification<Employee>();
specification.Conditions.Add(e => e.EmployeeName == "Tanvir");
specification.Includes = sp => sp.Include(e => e.Department);
specification.OrderBy = sp => sp.OrderBy(e => e.Salary);

Employee employee = await _repository.GetAsync<Employee>(specification);

Employee noTrackedEmployee = await _repository.GetAsync<Employee>(specification, true);
```

#### 11. To get a single projected entity by any condition/filter:

```C#
Expression<Func<Employee, object>> selectExpression = e => new { e.EmployeeId, e.EmployeeName };
var projectedEntity = await _repository.GetProjectedAsync<Employee, object>(e => e.EmployeeName == "Tanvir", selectExpression);
```
    
#### 12. To get a single projected entity by `Specification<T>`:

```C#
Specification<Employee> specification = new Specification<Employee>();
specification.Conditions.Add(e => e.EmployeeName == "Tanvir");
specification.Includes = sp => sp.Include(e => e.Department);
specification.OrderBy = sp => sp.OrderBy(e => e.Salary);

Expression<Func<Employee, object>> selectExpression = e => new { e.EmployeeId, e.EmployeeName };
var projectedEntity = await _repository.GetProjectedAsync<Employee, object>(specification, selectExpression);
```

#### 13. To check if an entity exists:

```C#
bool isExists = await _repository.ExistsAsync<Employee>(e => e.EmployeeName == "Tanvir");
```

#### 14. To create or insert a new entity:

```C#
Employee employeeToBeCreated = new Employee()
{
   EmployeeName = "Tanvir",
   ..........
}

await _repository.InsertAsync<Employee>(employeeToBeCreated);
```
    
#### 15. To create or insert a collection of new entities:

```C#
List<Employee> employeesToBeCreated = new List<Employee>()
{
   new Employee(){},
   new Employee(){},
}

await _repository.InsertAsync<Employee>(employeesToBeCreated);
```

#### 16. To update or modify an entity:

```C#
Employee employeeToBeUpdated = new Employee()
{
   EmployeeId = 1,
   EmployeeName = "Tanvir",
   ..........
}

await _repository.UpdateAsync<Employee>(employeeToBeUpdated);
```

#### 17. To update or modify the collection of entities:

```C#
List<Employee> employeesToBeUpdated = new List<Employee>()
{
   new Employee(){},
   new Employee(){},
}

await _repository.UpdateAsync<Employee>(employeesToBeUpdated);
```

#### 18. To delete an entity:

```C#
Employee employeeToBeDeleted = new Employee()
{
    EmployeeId = 1,
    EmployeeName = "Tanvir",
   ..........
}

await _repository.DeleteAsync<Employee>(employeeToBeDeleted);
```

#### 19. To delete a collection of entities:

```C#
List<Employee> employeesToBeDeleted = new List<Employee>()
{
   new Employee(){},
   new Employee(){},
}

await _repository.DeleteAsync<Employee>(employeesToBeDeleted);
```

#### 20. To get the count of entities with or without condition:

```C#
int count =   await _repository.GetCountAsync<Employee>(); // Count of all
int count =   await _repository.GetCountAsync<Employee>(e => e.EmployeeName = "Tanvir"); // Count with specified condtion

long longCount =   await _repository.GetLongCountAsync<Employee>(); // Long count of all
long longCount =   await _repository.GetLongCountAsync<Employee>(e => e.EmployeeName = "Tanvir"); // Long count with specified condtion
```
