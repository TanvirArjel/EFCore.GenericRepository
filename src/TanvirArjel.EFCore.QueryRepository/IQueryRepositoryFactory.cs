// <copyright file="IQueryRepositoryFactory.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Defines a factory for creating <see cref="DbContext" /> instances.
    /// </summary>
    /// <typeparam name="TContext">The <see cref="DbContext" /> type to create.</typeparam>
    public interface IQueryRepositoryFactory<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Creates a new <see cref="IQueryRepository{TContext}" /> instance.
        /// </summary>
        /// <remarks>
        /// The caller is responsible for disposing the repository; it will not be disposed by any dependency injection container.
        /// </remarks>
        /// <returns>A new repository instance.</returns>
        IQueryRepository<TContext> CreateQueryRepository();

        /// <summary>
        /// Creates a new <see cref="IQueryRepository{TContext}" /> instance in an async context.
        /// </summary>
        /// <remarks>
        /// The caller is responsible for disposing the repository; it will not be disposed by any dependency injection container.
        /// </remarks>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task containing the created repository that represents the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
        Task<IQueryRepository<TContext>> CreateQueryRepositoryAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(CreateQueryRepository());
    }
}