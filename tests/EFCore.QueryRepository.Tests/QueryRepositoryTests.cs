using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;
using Xunit;

namespace EFCore.QueryRepository.Tests;

public class QueryRepositoryTests : IAsyncLifetime
{
    private DemoDbContext _context;
    private QueryRepository<DemoDbContext> _queryRepository;

    private readonly List<Employee> fakeEmployees = new()
    {
        new()
        {
            Id = 1,
            DepartmentId = 1,
            Name = "Mark",
            Department = new Department
            {
                Id = 1,
                Name = "IT"
            }
        },
        new()
        {
            Id = 2,
            DepartmentId = 1,
            Name = "Merry",
            Department = new Department
            {
                Id = 2,
                Name = "HR"
            }
        }
    };

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<DemoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DemoDbContext(options);
        _queryRepository = new QueryRepository<DemoDbContext>(_context);
        await _context.Employees.AddRangeAsync(fakeEmployees);
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }

    #region GetQueryable Tests

    [Fact]
    public async Task GetQueryable_Test()
    {
        // Act
        IQueryable<Employee> queryable = _queryRepository.GetQueryable<Employee>();

        // Assert
        Assert.NotNull(queryable);
        Assert.Equal(fakeEmployees.Count, queryable.Count());
    }

    #endregion

    #region GetList Tests
    [Fact]
    public async Task GetListAsyncTest()
    {
        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>();

        // Assert
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.True(employees.Count == fakeEmployees.Count);
    }

    [Fact]
    public async Task GetListAsync_WithCondition_Test()
    {
        // Arrange
        int expectedCount = fakeEmployees.Where(e => e.Id == 1).Count();
        string expectedName = "Mark";

        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>(e => e.Id == 1);

        // Assert
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.True(employees.Count == expectedCount);
        Assert.Equal(expectedName, employees.First().Name);
    }

    [Fact]
    public async Task GetListAsync_WithInclude_Test()
    {
        // Arrange
        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>(q => q.Include(e => e.Department));

        // Assert
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.True(employees.Count == fakeEmployees.Count);
    }


    [Fact]
    public async Task GetListAsync_NoTracking_Test()
    {
        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>(asNoTracking: true);

        // Assert
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.Equal(fakeEmployees.Count, employees.Count);
    }

    [Fact]
    public async Task GetListAsync_WithIncludeAndNoTracking_Test()
    {
        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>(
            q => q.Include(e => e.Department),
            asNoTracking: true);

        // Assert
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.Equal(fakeEmployees.Count, employees.Count);
    }

    [Fact]
    public async Task GetListAsync_WithConditionAndNoTracking_Test()
    {
        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>(
            e => e.Id == 1,
            asNoTracking: true);

        // Assert
        Assert.NotNull(employees);
        Assert.Single(employees);
        Assert.Equal("Mark", employees.First().Name);
    }

    [Fact]
    public async Task GetListAsync_WithConditionAndIncludeAndNoTracking_Test()
    {
        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>(
            e => e.DepartmentId == 1,
            q => q.Include(e => e.Department),
            asNoTracking: true);

        // Assert
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.All(employees, e => Assert.Equal(1, e.DepartmentId));
    }

    [Fact]
    public async Task GetListAsync_Projected_Test()
    {
        // Act
        List<string> names = await _queryRepository.GetListAsync<Employee, string>(
            e => e.Name);

        // Assert
        Assert.NotNull(names);
        Assert.NotEmpty(names);
        Assert.Equal(fakeEmployees.Count, names.Count);
    }

    [Fact]
    public async Task GetListAsync_ProjectedWithCondition_Test()
    {
        // Act
        List<string> names = await _queryRepository.GetListAsync<Employee, string>(
            e => e.Id == 1,
            e => e.Name);

        // Assert
        Assert.NotNull(names);
        Assert.Single(names);
        Assert.Equal("Mark", names.First());
    }

    [Fact]
    public async Task GetListAsync_WithSpecification_Test()
    {
        // Arrange
        var spec = new Specification<Employee>();
        spec.Conditions.Add(e => e.DepartmentId == 1);

        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>(spec);

        // Assert
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
    }

    [Fact]
    public async Task GetListAsync_WithSpecificationAndNoTracking_Test()
    {
        // Arrange
        var spec = new Specification<Employee>();
        spec.Conditions.Add(e => e.Id == 1);

        // Act
        List<Employee> employees = await _queryRepository.GetListAsync<Employee>(spec, asNoTracking: true);

        // Assert
        Assert.NotNull(employees);
        Assert.Single(employees);
    }

    [Fact]
    public async Task GetListAsync_WithSpecificationProjected_Test()
    {
        // Arrange
        var spec = new Specification<Employee>();
        spec.Conditions.Add(e => e.DepartmentId == 1);

        // Act
        List<string> names = await _queryRepository.GetListAsync<Employee, string>(
            spec,
            e => e.Name);

        // Assert
        Assert.NotNull(names);
        Assert.NotEmpty(names);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetByIdAsync_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetByIdAsync<Employee>(1);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal(1, employee.Id);
        Assert.Equal("Mark", employee.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithNoTracking_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetByIdAsync<Employee>(1, asNoTracking: true);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal(1, employee.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInclude_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetByIdAsync<Employee>(
            1,
            q => q.Include(e => e.Department),
            asNoTracking: false);

        // Assert
        Assert.NotNull(employee);
        Assert.NotNull(employee.Department);
        Assert.Equal(1, employee.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithIncludeAndNoTracking_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetByIdAsync<Employee>(
            1,
            q => q.Include(e => e.Department),
            asNoTracking: true);

        // Assert
        Assert.NotNull(employee);
        Assert.NotNull(employee.Department);
    }

    [Fact(Skip = "In-memory provider does not support this query pattern")]
    public async Task GetByIdAsync_Projected_Test()
    {
        // Act
        string name = await _queryRepository.GetByIdAsync<Employee, string>(
            1,
            e => e.Name);

        // Assert
        Assert.NotNull(name);
        Assert.Equal("Mark", name);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetByIdAsync<Employee>(999);

        // Assert
        Assert.Null(employee);
    }

    #endregion

    #region Get Tests

    [Fact]
    public async Task GetAsync_WithCondition_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetAsync<Employee>(e => e.Name == "Mark");

        // Assert
        Assert.NotNull(employee);
        Assert.Equal("Mark", employee.Name);
    }

    [Fact]
    public async Task GetAsync_WithConditionAndNoTracking_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetAsync<Employee>(
            e => e.Name == "Merry",
            asNoTracking: true);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal("Merry", employee.Name);
    }

    [Fact]
    public async Task GetAsync_WithConditionAndInclude_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetAsync<Employee>(
            e => e.Id == 1,
            q => q.Include(e => e.Department));

        // Assert
        Assert.NotNull(employee);
        Assert.NotNull(employee.Department);
    }

    [Fact]
    public async Task GetAsync_WithConditionIncludeAndNoTracking_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetAsync<Employee>(
            e => e.Id == 2,
            q => q.Include(e => e.Department),
            asNoTracking: true);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal(2, employee.Id);
    }

    [Fact]
    public async Task Get_WithCondition_Test()
    {
        // Arrange
        var spec = new Specification<Employee>();
        spec.Conditions.Add(e => e.Name == "Mark");

        // Act
        Employee employee = await _queryRepository.GetAsync<Employee>(spec);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal("Mark", employee.Name);
    }

    [Fact]
    public async Task GetAsync_WithSpecificationAndNoTracking_Test()
    {
        // Arrange
        var spec = new Specification<Employee>();
        spec.Conditions.Add(e => e.Id == 1);

        // Act
        Employee employee = await _queryRepository.GetAsync<Employee>(spec, asNoTracking: true);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal(1, employee.Id);
    }

    [Fact]
    public async Task GetAsync_ProjectedWithCondition_Test()
    {
        // Act
        string name = await _queryRepository.GetAsync<Employee, string>(
            e => e.Id == 1,
            e => e.Name);

        // Assert
        Assert.NotNull(name);
        Assert.Equal("Mark", name);
    }

    [Fact]
    public async Task GetAsync_WithSpecification_Test()
    {
        // Arrange
        var spec = new Specification<Employee>();
        spec.Conditions.Add(e => e.Id == 2);

        // Act
        string name = await _queryRepository.GetAsync<Employee, string>(
            spec,
            e => e.Name);

        // Assert
        Assert.NotNull(name);
        Assert.Equal("Merry", name);
    }

    [Fact]
    public async Task GetAsync_NotFound_Test()
    {
        // Act
        Employee employee = await _queryRepository.GetAsync<Employee>(e => e.Name == "NonExistent");

        // Assert
        Assert.Null(employee);
    }

    #endregion

    #region Exists Tests

    [Fact]
    public async Task ExistsAsync_NoCondition_Test()
    {
        // Act
        bool exists = await _queryRepository.ExistsAsync<Employee>();

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_WithCondition_Test()
    {
        // Act
        bool exists = await _queryRepository.ExistsAsync<Employee>(e => e.Name == "Mark");

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_WithConditionNotMatching_Test()
    {
        // Act
        bool exists = await _queryRepository.ExistsAsync<Employee>(e => e.Name == "NonExistent");

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsByIdAsync_Test()
    {
        // Act
        bool exists = await _queryRepository.ExistsByIdAsync<Employee>(1);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsByIdAsync_NotFound_Test()
    {
        // Act
        bool exists = await _queryRepository.ExistsByIdAsync<Employee>(999);

        // Assert
        Assert.False(exists);
    }

    #endregion

    #region Count Tests

    [Fact]
    public async Task GetCountAsync_NoCondition_Test()
    {
        // Act
        int count = await _queryRepository.GetCountAsync<Employee>();

        // Assert
        Assert.Equal(fakeEmployees.Count, count);
    }

    [Fact]
    public async Task GetCountAsync_WithCondition_Test()
    {
        // Act
        int count = await _queryRepository.GetCountAsync<Employee>(e => e.DepartmentId == 1);

        // Assert
        Assert.Equal(fakeEmployees.Where(e => e.DepartmentId == 1).Count(), count);
    }

    [Fact]
    public async Task GetCountAsync_WithMultipleConditions_Test()
    {
        // Arrange
        var conditions = new List<Expression<Func<Employee, bool>>>
        {
            e => e.DepartmentId == 1,
            e => e.Id > 0
        };

        // Act
        int count = await _queryRepository.GetCountAsync<Employee>(conditions);

        // Assert
        Assert.True(count > 0);
    }

    [Fact]
    public async Task GetLongCountAsync_NoCondition_Test()
    {
        // Act
        long count = await _queryRepository.GetLongCountAsync<Employee>();

        // Assert
        Assert.Equal(fakeEmployees.Count, count);
    }

    [Fact]
    public async Task GetLongCountAsync_WithCondition_Test()
    {
        // Act
        long count = await _queryRepository.GetLongCountAsync<Employee>(e => e.DepartmentId == 1);

        // Assert
        Assert.True(count > 0);
    }

    [Fact]
    public async Task GetLongCountAsync_WithMultipleConditions_Test()
    {
        // Arrange
        var conditions = new List<Expression<Func<Employee, bool>>>
        {
            e => e.Id > 0,
            e => e.DepartmentId >= 1
        };

        // Act
        long count = await _queryRepository.GetLongCountAsync<Employee>(conditions);

        // Assert
        Assert.True(count > 0);
    }

    #endregion

    #region Raw SQL Tests

    [Fact(Skip = "Raw SQL queries require a relational database provider")]
    public async Task GetFromRawSqlAsync_NoParameters_Test()
    {
        // Arrange
        string sql = "SELECT * FROM Employees";

        // Act
        List<Employee> employees = await _queryRepository.GetFromRawSqlAsync<Employee>(sql);

        // Assert
        Assert.NotNull(employees);
    }

    [Fact(Skip = "Raw SQL queries require a relational database provider")]
    public async Task GetFromRawSqlAsync_WithSingleParameter_Test()
    {
        // Arrange
        string sql = "SELECT * FROM Employees WHERE Id = @p0";

        // Act
        List<Employee> employees = await _queryRepository.GetFromRawSqlAsync<Employee>(sql, 1);

        // Assert
        Assert.NotNull(employees);
    }

    [Fact(Skip = "Raw SQL queries require a relational database provider")]
    public async Task GetFromRawSqlAsync_WithMultipleParameters_Test()
    {
        // Arrange
        string sql = "SELECT * FROM Employees WHERE Id > @p0 AND DepartmentId = @p1";
        var parameters = new List<object> { 0, 1 };

        // Act
        List<Employee> employees = await _queryRepository.GetFromRawSqlAsync<Employee>(sql, parameters);

        // Assert
        Assert.NotNull(employees);
    }

    #endregion

    #region Pagination Tests

    [Fact]
    public async Task GetListAsync_WithPagination_Test()
    {
        // Arrange
        var paginationSpec = new PaginationSpecification<Employee>
        {
            PageIndex = 1,
            PageSize = 1
        };

        // Act
        PaginatedList<Employee> result = await _queryRepository.GetListAsync<Employee>(paginationSpec);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task GetListAsync_WithPaginationProjected_Test()
    {
        // Arrange
        var paginationSpec = new PaginationSpecification<Employee>
        {
            PageIndex = 1,
            PageSize = 1
        };

        // Act
        PaginatedList<string> result = await _queryRepository.GetListAsync<Employee, string>(
            paginationSpec,
            e => e.Name);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
    }

    #endregion
}