using Microsoft.EntityFrameworkCore;
using Moq;
using TanvirArjel.EFCore.GenericRepository;

namespace EFCore.GenericRepository.Tests;

public class RepositoryFactoryTests
{
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDbContextFactoryIsNull()
    {
        // Arrange
        IDbContextFactory<DbContext> dbContextFactory = null;

        // Act & Assert
        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Throws<ArgumentNullException>(
            "dbContextFactory",
            () => new RepositoryFactory<DbContext>(dbContextFactory));
    }

    [Fact]
    public void CreateQueryRepository_ShouldReturnQueryRepositoryInstance()
    {
        // Arrange
        var dbContextFactoryMock = new Mock<IDbContextFactory<DbContext>>();
        var dbContextMock = new Mock<DbContext>();

        dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(dbContextMock.Object);

        var repositoryFactory = new RepositoryFactory<DbContext>(dbContextFactoryMock.Object);

        // Act
        var queryRepository = repositoryFactory.CreateRepository();

        // Assert
        Assert.IsType<Repository<DbContext>>(queryRepository);
    }

    [Fact]
    public void CreateQueryRepository_ShouldDisposeQueryRepository()
    {
        // Arrange
        var dbContextFactoryMock = new Mock<IDbContextFactory<DbContext>>();
        var dbContextMock = new Mock<DbContext>();

        dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(dbContextMock.Object);

        var repositoryFactory = new RepositoryFactory<DbContext>(dbContextFactoryMock.Object);
        var queryRepository = repositoryFactory.CreateRepository();

        // Act
        queryRepository.Dispose();

        // Assert
        dbContextMock.Verify(db => db.Dispose(), Times.Once);
    }

    [Fact]
    public async Task CreateQueryRepository_ShouldDisposeQueryRepositoryAsync()
    {
        // Arrange
        var dbContextFactoryMock = new Mock<IDbContextFactory<DbContext>>();
        var dbContextMock = new Mock<DbContext>();

        dbContextFactoryMock.Setup(factory => factory.CreateDbContext()).Returns(dbContextMock.Object);

        var repositoryFactory = new RepositoryFactory<DbContext>(dbContextFactoryMock.Object);
        var queryRepository = repositoryFactory.CreateRepository();

        // Act
        await queryRepository.DisposeAsync();

        // Assert
        dbContextMock.Verify(db => db.DisposeAsync(), Times.Once);
    }
}