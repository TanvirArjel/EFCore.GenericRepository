// <copyright file="ServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Contain all the service collection extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add generic repository services to the .NET Dependency Injection container.
        /// </summary>
        /// <typeparam name="TDbContext">Your EF Core <see cref="DbContext"/>.</typeparam>
        /// <param name="services">The type to be extended.</param>
        /// <param name="lifetime">The life time of the service.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddGenericRepository<TDbContext>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(
                typeof(IRepository),
                serviceProvider =>
                {
                    TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                    return new Repository<TDbContext>(dbContext);
                },
                lifetime));

            services.Add(new ServiceDescriptor(
               typeof(IRepository<TDbContext>),
               serviceProvider =>
               {
                   TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                   return new Repository<TDbContext>(dbContext);
               },
               lifetime));

            return services;
        }

        /// <summary>
        ///     Registers an <see cref="IRepositoryFactory{TDbContext}" /> in the <see cref="IServiceCollection" /> to create instances
        ///     of given <see cref="IRepository{TDbContext}" /> type.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Registering a factory instead of registering the reposiotry type directly allows for easy creation of new
        ///         <see cref="IRepository{TDbContext}" /> instances.
        ///         Registering a factory is recommended for Blazor applications and other situations where the dependency
        ///         injection scope is not aligned with the repository lifetime.
        ///     </para>
        ///     <para>
        ///         Use this method when using dependency injection in your application, such as with Blazor.
        ///         For applications that don't use dependency injection, consider creating <see cref="IRepository{TDbContext}" />
        ///         instances directly with its constructor.
        ///     </para>
        ///     <para>
        ///         Entity Framework Core does not support multiple parallel operations being run on the same <see cref="DbContext" />
        ///         instance. This includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
        ///         Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
        ///         in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information
        ///         and examples.
        ///     </para>
        ///     <para>
        ///         See <see href="https://aka.ms/efcore-docs-di">Using DbContext with dependency injection</see> and
        ///         <see href="https://aka.ms/efcore-docs-dbcontext-factory">Using DbContext factories</see> for more information and examples.
        ///     </para>
        /// </remarks>
        /// <typeparam name="TDbContext">The type of <see cref="DbContext" /> to be created by the factory.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="lifetime">
        ///     The lifetime with which to register the factory and options.
        ///     The default is <see cref="ServiceLifetime.Singleton" />.
        /// </param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddGenericRepositoryFactory<TDbContext>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TDbContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(
                typeof(IRepositoryFactory<TDbContext>),
                typeof(RepositoryFactory<TDbContext>),
                lifetime));

            return services;
        }
    }
}
