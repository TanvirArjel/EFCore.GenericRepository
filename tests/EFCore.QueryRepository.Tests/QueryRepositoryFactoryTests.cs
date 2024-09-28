using Microsoft.EntityFrameworkCore;
using Moq;
using TanvirArjel.EFCore.GenericRepository;

namespace EFCore.QueryRepository.Tests;

public class QueryRepositoryFactoryTests
{
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDbContextFactoryIsNull()
    {
        // Arrange
        IDbContextFactory<DemoDbContext> dbContextFactory = null;

        // Act & Assert
        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Throws<ArgumentNullException>(
            "dbContextFactory",
            () => new QueryRepositoryFactory<DemoDbContext>(dbContextFactory));
    }

    [Fact]
    public void CreateQueryRepository_ShouldReturnQueryRepositoryInstance()
    {
        // Arrange
        var dbContextFactoryMock = new Mock<IDbContextFactory<DemoDbContext>>();
        var dbContextMock = new Mock<DemoDbContext>();

        dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(dbContextMock.Object);

        var queryRepositoryFactory = new QueryRepositoryFactory<DemoDbContext>(dbContextFactoryMock.Object);

        // Act
        var queryRepository = queryRepositoryFactory.CreateQueryRepository();

        // Assert
        Assert.IsType<QueryRepository<DemoDbContext>>(queryRepository);
    }

    [Fact]
    public void CreateQueryRepository_ShouldDisposeQueryRepository()
    {
        // Arrange
        var dbContextFactoryMock = new Mock<IDbContextFactory<DemoDbContext>>();
        var dbContextMock = new Mock<DemoDbContext>();

        dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(dbContextMock.Object);

        var queryRepositoryFactory = new QueryRepositoryFactory<DemoDbContext>(dbContextFactoryMock.Object);
        var queryRepository = queryRepositoryFactory.CreateQueryRepository();

        // Act
        queryRepository.Dispose();

        // Assert
        dbContextMock.Verify(db => db.Dispose(), Times.Once);
    }

    [Fact]
    public async Task CreateQueryRepository_ShouldDisposeQueryRepositoryAsync()
    {
        // Arrange
        var dbContextFactoryMock = new Mock<IDbContextFactory<DemoDbContext>>();
        var dbContextMock = new Mock<DemoDbContext>();

        dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(dbContextMock.Object);

        var queryRepositoryFactory = new QueryRepositoryFactory<DemoDbContext>(dbContextFactoryMock.Object);
        var queryRepository = queryRepositoryFactory.CreateQueryRepository();

        // Act
        await queryRepository.DisposeAsync();

        // Assert
        dbContextMock.Verify(db => db.DisposeAsync(), Times.Once);
    }
}