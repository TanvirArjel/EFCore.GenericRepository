using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TanvirArjel.EFCore.GenericRepository;

namespace EFCore.GenericRepository.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddGenericRepository_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        // Arrange
        IServiceCollection services = null;

        // Act & Assert
        // ReSharper disable once ExpressionIsAlwaysNull
        Assert.Throws<ArgumentNullException>(() => services.AddGenericRepository<DemoDbContext>());
    }

    [Fact]
    public void AddGenericRepository_ShouldAddServicesToServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddDbContext<DemoDbContext>(x => x.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        // Act
        services.AddGenericRepository<DemoDbContext>();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var queryRepository = serviceProvider.GetService<IRepository<DemoDbContext>>();
        Assert.NotNull(queryRepository);
        Assert.IsType<Repository<DemoDbContext>>(queryRepository);
    }

    [Fact]
    public void AddGenericRepositoryFactory_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        // Arrange
        IServiceCollection services = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddGenericRepositoryFactory<DemoDbContext>());
    }

    [Fact]
    public void AddGenericRepositoryFactory_ShouldAddFactoryToServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddDbContextFactory<DemoDbContext>(x => x.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        // Act
        services.AddGenericRepositoryFactory<DemoDbContext>();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetService<IRepositoryFactory<DemoDbContext>>();

        Assert.NotNull(factory);
        Assert.IsType<RepositoryFactory<DemoDbContext>>(factory);
    }
}