// <copyright file="ServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TanvirArjel.EFCore.GenericRepository.Implementations;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Contain all the service collection extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register all the necessary services to the ASP.NET Core Dependency Injection container.
        /// </summary>
        /// <typeparam name="TDbContext">Your EF Core DbContext.</typeparam>
        /// <param name="services">The type to be extended.</param>
        /// <param name="lifetime">The life time of the service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is NULL.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <typeparamref name="TDbContext"/> is NULL.</exception>
        public static void AddGenericRepository<TDbContext>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(
                typeof(IRepository),
                serviceProvider => new Repository(ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider)),
                lifetime));
        }
    }
}
