// <copyright file="ServiceCollectionExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using EFCore.GenericRepository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.GenericRepository
{
    /// <summary>
    /// Contain all the service collection extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register all the necessary TanvirArjelExceptionHandler services to the ASP.NET Core Dependency Injection container
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance this method extends.</param>
        /// <param name="dbContext">Connection String of the <c>SQL Server</c> database to which exception will logged.</param>
        /// <exception cref="ArgumentNullException"> Thrown if <c>SQL Server</c> connection string is null or empty.</exception>
        public static void AddGenericRepository(this IServiceCollection services, DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>(uow => new UnitOfWork.UnitOfWork(dbContext));
        }
    }
}
