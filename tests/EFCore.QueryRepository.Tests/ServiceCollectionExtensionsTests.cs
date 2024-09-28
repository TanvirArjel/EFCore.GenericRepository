using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TanvirArjel.EFCore.GenericRepository;

namespace EFCore.QueryRepository.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddQueryRepository_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        // Arrange
        IServiceCollection services = null;

        // Act & Assert
        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Throws<ArgumentNullException>(() => services.AddQueryRepository<DemoDbContext>());
    }

    [Fact]
    public void AddQueryRepository_ShouldAddServicesToServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddDbContext<DemoDbContext>(x => x.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        // Act
        services.AddQueryRepository<DemoDbContext>();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var queryRepository = serviceProvider.GetService<IQueryRepository<DemoDbContext>>();

        Assert.IsType<QueryRepository<DemoDbContext>>(queryRepository);
    }

    [Fact]
    public void AddGenericRepositoryFactory_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        // Arrange
        IServiceCollection services = null;

        // Act & Assert
        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Throws<ArgumentNullException>(() => services.AddQueryRepositoryFactory<DemoDbContext>());
    }

    [Fact]
    public void AddGenericRepositoryFactory_ShouldAddFactoryToServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddDbContextFactory<DemoDbContext>(x => x.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        
        // Act
        services.AddQueryRepositoryFactory<DemoDbContext>();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetService<IQueryRepositoryFactory<DemoDbContext>>();

        Assert.NotNull(factory);
        Assert.IsType<QueryRepositoryFactory<DemoDbContext>>(factory);
    }
}