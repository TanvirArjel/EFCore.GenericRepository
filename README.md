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

First install the lastest version of `EFCore.GenericRepository` nuget package into your project as follows:

    Install-Package EFCore.GenericRepository
    
Then in the `ConfirugeServices` method of the `Startup` class:

    public void ConfigureServices(IServiceCollection services)
    {
        string connectionString = Configuration.GetConnectionString("RepositoryDbConnection");
        services.AddDbContext<DemoDbContext>(option => option.UseSqlServer(connectionString));
        services.AddGenericRepository<DemoDbContext>(); // Call it just after registering your DbConext.
    }
    
## Usage:

#### 1. To get all the data of the table:



