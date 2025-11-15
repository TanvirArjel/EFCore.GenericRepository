using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;

namespace EFCore.QueryRepository.Tests;

public class QueryRepositoryTests
{
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

    [Fact]
    public async Task GetListAsyncTest()
    {
        // Arrange
        Mock<DemoDbContext> mockDemoContext = new();
        mockDemoContext.Setup(context => context.Set<Employee>()).ReturnsDbSet(fakeEmployees);

        // Act
        QueryRepository<DemoDbContext> queryRepository = new(mockDemoContext.Object);
        List<Employee> employees = await queryRepository.GetListAsync<Employee>();

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

        Mock<DemoDbContext> mockDemoContext = new();
        mockDemoContext.Setup(context => context.Set<Employee>()).ReturnsDbSet(fakeEmployees);

        // Act
        QueryRepository<DemoDbContext> queryRepository = new(mockDemoContext.Object);
        List<Employee> employees = await queryRepository.GetListAsync<Employee>(e => e.Id == 1);

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
        Mock<DemoDbContext> mockDemoContext = new();
        mockDemoContext.Setup(context => context.Set<Employee>()).ReturnsDbSet(fakeEmployees);

        // Act
        QueryRepository<DemoDbContext> queryRepository = new(mockDemoContext.Object);
        List<Employee> employees = await queryRepository.GetListAsync<Employee>(q => q.Include(e => e.Department));

        // Assert
        Assert.NotNull(employees);
        Assert.NotEmpty(employees);
        Assert.True(employees.Count == fakeEmployees.Count);
    }

    [Fact]
    public void Dispose_ShouldDisposeDbContext()
    {
        // Arrange
        var dbContextMock = new Mock<DemoDbContext>();
        var queryRepository = new QueryRepository<DemoDbContext>(dbContextMock.Object);

        // Act
        queryRepository.Dispose();

        // Assert
        dbContextMock.Verify(db => db.Dispose(), Times.Once);
    }

    [Fact]
    public async Task DisposeAsync_ShouldDisposeDbContextAsync()
    {
        // Arrange
        var dbContextMock = new Mock<DemoDbContext>();
        var queryRepository = new QueryRepository<DemoDbContext>(dbContextMock.Object);

        // Act
        await queryRepository.DisposeAsync();

        // Assert
        dbContextMock.Verify(db => db.DisposeAsync(), Times.Once);
    }
}