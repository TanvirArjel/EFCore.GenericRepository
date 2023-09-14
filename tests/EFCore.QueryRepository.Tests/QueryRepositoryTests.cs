using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;

namespace EFCore.QueryRepository.Tests;

public class QueryRepositoryTests
{
    [Fact]
    public async Task GetListAsyncTest()
    {
        // Arrange
        var fakeEmployees = new List<Employee>()
        {
            new() {Id = 1,Name = "Mark"},
            new() {Id = 1, Name = "Merry"}
        };

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
        var fakeEmployees = new List<Employee>()
        {
            new() {Id = 1,Name = "Mark"},
            new() {Id = 2, Name = "Merry"}
        };

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
}