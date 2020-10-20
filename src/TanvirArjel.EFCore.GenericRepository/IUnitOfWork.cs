// <copyright file="IUnitOfWork.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Contains all the UnitOfWork services.
    /// </summary>
    [Obsolete("This will be removed in version 6.0.0. Please use IRepository interface instead.")]
    public interface IUnitOfWork
    {
        /// <summary>
        /// Returns the repository for the provided type.
        /// </summary>
        /// <typeparam name="T">The database entity type.</typeparam>
        /// <returns>Returns <see cref="Repository{T}"/>.</returns>
        IUnitOfWorkRepository<T> Repository<T>()
            where T : class;

        /// <summary>
        /// Execute raw sql command against the configured database.
        /// </summary>
        /// <param name="sql">The sql string.</param>
        /// <param name="parameters">The paramters in the sql string.</param>
        /// <returns>Returns <see cref="int"/>.</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// Execute raw sql command against the configured database asynchronously.
        /// </summary>
        /// <param name="sql">The sql string.</param>
        /// <param name="parameters">The paramters in the sql string.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

        /// <summary>
        /// Reset the DbContext state by removing all the tracked and attached entities.
        /// </summary>
        void ResetContextState();

        /// <summary>
        /// Trigger the execution of the EF core commands against the configuired database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
