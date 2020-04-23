# EF Core Generic Repository

This library is a perfect Generic Repository implementation for EF Core ORM which will remove your pain to write repository layer for your .NET Core or .NET project.

## This library includes following notable features:

1. This library can be run on any .NET Core or .NET application which has .NET Standard 2.0 and .NET Standard 2.1 support.

2. It’s providing the Generic Repository through Unit of Work pattern.

3. It has all the required methods to query your data in whatever you want without getting IQueryable<T> from the repository.

4. It also has **Specification<T>** pattern support so that you can build your query dynamically i.e. differed query building.

5. It has also database level projection support for your query.

6. It also has support to run raw SQL command against your relational database.

7. It also has support to choose whether you would like track your query entities or not.

8. It has also support to reset your EF Core DbContext state whenever you really needed.

9.  Most importantly, it has full Unit Testing support.

## How do I get started?

First install the appropriate version of `EFCore.GenericRepository` nuget package into your project as follows:

**For EF Core 2.x :**

    Install-Package EFCore.GenericRepository -Version 2.0.0
    
**For EF Core 3.0 :**

    Install-Package EFCore.GenericRepository -Version 3.0.0
    
**For EF Core >= 3.1 :**

    Install-Package EFCore.GenericRepository -Version 3.1.0
    
Then in the `ConfirugeServices` method of the `Startup` class:

    public void ConfigureServices(IServiceCollection services)
    {
        string connectionString = Configuration.GetConnectionString("RepositoryDbConnection");
        services.AddDbContext<DemoDbContext>(option => option.UseSqlServer(connectionString));
        services.AddGenericRepository<DemoDbContext>(); // Call it just after registering your DbConext.
    }
    
## Usage:

#### 1. To get all the data:

    var employeeList =  await _unitOfWork.Repository<Employee>().GetEntityListAsync();
    
    var noTrackedEmployeeList = await _unitOfWork.Repository<Employee>().GetEntityListAsync(asNoTracking: true);
    
#### 2. To get filtered list of data:

    var employeeList =  await _unitOfWork.Repository<Employee>()
                        .GetEntityListAsync(e => e.EmployeeName.Contains("Tanvir") && e.DepartmentName == "Software");
                        
    var noTrackedEmployeeList = await _unitOfWork.Repository<Employee>()
                                .GetEntityListAsync(e => e.EmployeeName.Contains("Tanvir") && e.DepartmentName == "Software", asNoTracking: true);

#### 3. To get list of data by Specification<T>:
    
    Specification<Employee> specification = new Specification<Employee>();
    specification.Conditions.Add(e => e.EmployeeName.Contains("Tanvir"));
    specification.Includes = ep => ep.Include(e => e.Department);
    specification.OrderBy = sp => sp.OrderBy(e => e.EmployeeName).ThenBy(e => e.DepartmentName);
    specification.Skip = 0;
    specification.Take = 10;

    List<Employee> employeeList = await _unitOfWork.Repository<Employee>()
                                  .GetEntityListAsync(specification);
                                  
    List<Employee> noTrackedEmployeeList = await _unitOfWork.Repository<Employee>()
                                  .GetEntityListAsync(specification);
                                  
 #### 4. To get projected entity list:
 
    var projectedList = await _unitOfWork.Repository<Employee>()
                      .GetProjectedEntityListAsync(e => new { e.EmployeeId, e.EmployeeName});
                      
 #### 5. To get filtered projected entity list:
 
    var filteredProjectedList = await _unitOfWork.Repository<Employee>()
                      .GetProjectedEntityListAsync(e => e.IsActive, e => new { e.EmployeeId, e.EmployeeName});
                                            
 #### 6. To get projected entity list by Specification<T>:
 
    Specification<Employee> specification = new Specification<Employee>();
    specification.Conditions.Add(e => e.EmployeeName.Contains("Tanvir"));
    specification.Includes = ep => ep.Include(e => e.Department);
    specification.OrderBy = sp => sp.OrderBy(e => e.EmployeeName).ThenBy(e => e.DepartmentName);
    specification.Skip = 0;
    specification.Take = 10;
    
    var projectedList = await _unitOfWork.Repository<Employee>()
                      .GetProjectedEntityListAsync(specification, e => new { e.EmployeeId, e.EmployeeName});
                      
#### 7. To get entity by Id (primary key):

    Employee employee = await _unitOfWork.Repository<Employee>().GetEntityByIdAsync(1);
    Employee noTrackedEmployee = await _unitOfWork.Repository<Employee>().GetEntityByIdAsync(1, true);
    
#### 8. To get projected entity by Id (primary key):

    var projectedEntity = await _unitOfWork.Repository<Employee>().GetEntityByIdAsync(1,e => new { e.EmployeeId, e.EmployeeName});

#### 9. To get single entity by any condition / filter:

    Employee employee = await _unitOfWork.Repository<Employee>().GetEntityAsync(e => e.EmployeeName == "Tanvir");
    Employee noTrackedEmployee = await _unitOfWork.Repository<Employee>().GetEntityAsync(e => e.EmployeeName == "Tanvir", true);
    
#### 10. To get single entity by Specification<T>:
    
    Specification<Employee> specification = new Specification<Employee>();
    specification.Conditions.Add(e => e.EmployeeName == "Tanvir");
    specification.Includes = sp => sp.Include(e => e.Department);
    specification.OrderBy = sp => sp.OrderBy(e => e.Salary);
    
    Employee employee = await _unitOfWork.Repository<Employee>().GetEntityAsync(specification);
    Employee noTrackedEmployee = await _unitOfWork.Repository<Employee>().GetEntityAsync(specification, true);
