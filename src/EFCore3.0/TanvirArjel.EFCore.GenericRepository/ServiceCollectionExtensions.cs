// <copyright file="ServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TanvirArjel.EFCore.GenericRepository.Implementations;
using TanvirArjel.EFCore.GenericRepository.Services;

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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is NULL.</exception>
        /// <exception cref="ApplicationException">Thrown if <typeparamref name="TDbContext"/> is NULL.</exception>
        public static void AddGenericRepository<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            DbContext dbContext = serviceProvider.GetService<TDbContext>();

            if (dbContext == null)
            {
                throw new ApplicationException($"Please register your {typeof(TDbContext)} before calling {nameof(AddGenericRepository)}.");
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>(uow => new UnitOfWork(dbContext));
        }
    }
}
