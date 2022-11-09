// <copyright file="IRepository.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Contains all the repository methods. If you register the multiple DbContexts, it will use the last one.
    /// To use specific <see cref="DbContext"/> please use <see cref="IRepository{TDbContext}"/>.
    /// </summary>
    public interface IRepository : IQueryRepository
    {
        /// <summary>
        /// Begin a new database transaction.
        /// </summary>
        /// <param name="isolationLevel"><see cref="IsolationLevel"/> to be applied on this transaction. (Default to <see cref="IsolationLevel.Unspecified"/>).</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a <see cref="IDbContextTransaction"/> instance.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/>, insert it into database and returns <see cref="Task{TResult}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be inserted.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        [Obsolete("The method will be removed in the next version. Please use Add()/AddAsync() and SaveChangesAsync() methods together.")]
        Task<object[]> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/>, insert it into the database and returns <see cref="Task"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities to be inserted.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        [Obsolete("The method will be removed in the next version. Please use Add()/AddAsync and SaveChangesAsync() methods together.")]
        Task InsertAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/>, send update operation to the database and returns <see cref="void"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be updated.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        [Obsolete("The method will be removed in the next version. Please use Update() and SaveChangesAsync() methods together.")]
        Task<int> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> of entities, send update operation to the database and returns <see cref="void"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities to be updated.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        [Obsolete("The method will be removed in the next version. Please use Update() and SaveChangesAsync() methods together.")]
        Task<int> UpdateAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// This method takes an entity of type <typeparamref name="TEntity"/>, delete the entity from database and returns <see cref="void"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be deleted.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        [Obsolete("The method will be removed in the next version. Please use Remove() and SaveChangesAsync() methods together.")]
        Task<int> DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{T}"/> of entities, delete those entities from database and returns <see cref="void"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The list of entities to be deleted.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        [Obsolete("The method will be removed in the next version. Please use Remove() and SaveChangesAsync() methods together.")]
        Task<int> DeleteAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// Execute raw sql command against the configured database asynchronously.
        /// </summary>
        /// <param name="sql">The sql string.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute raw sql command against the configured database asynchronously.
        /// </summary>
        /// <param name="sql">The sql string.</param>
        /// <param name="parameters">The paramters in the sql string.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

        /// <summary>
        /// Execute raw sql command against the configured database asynchronously.
        /// </summary>
        /// <param name="sql">The sql string.</param>
        /// <param name="parameters">The paramters in the sql string.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reset the DbContext state by removing all the tracked and attached entities.
        /// </summary>
        void ClearChangeTracker();

        // Newly added methods

        /// <summary>
        /// This method takes an <typeparamref name="TEntity"/> object, mark the object as <see cref="EntityState.Added"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entity"/> to be added.</typeparam>
        /// <param name="entity">The <typeparamref name="TEntity"/> object to be inserted to the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Add<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// This method takes an object of <typeparamref name="TEntity"/>, adds it to the change tracker and will
        /// be inserted into the database when <see cref="IRepository.SaveChangesAsync(CancellationToken)" /> is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be inserted.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> objects, mark the objects as <see cref="EntityState.Added"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entities"/> to be added.</typeparam>
        /// <param name="entities">The <typeparamref name="TEntity"/> objects to be inserted to the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Add<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class;

        /// <summary>
        /// This method takes a collection of <typeparamref name="TEntity"/> object, adds them to the change tracker and will
        /// be inserted into the database when <see cref="IRepository.SaveChangesAsync(CancellationToken)" /> is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities to be inserted.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task AddAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class;

        /// <summary>
        /// This method takes an <typeparamref name="TEntity"/> object, mark the object as <see cref="EntityState.Modified"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entity"/> to be marked as modified.</typeparam>
        /// <param name="entity">The <typeparamref name="TEntity"/> object to be updated to the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Update<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> objects, mark the objects as <see cref="EntityState.Modified"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entities"/> to be marked as modified.</typeparam>
        /// <param name="entities">The entity objects to be updated to the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Update<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class;

        /// <summary>
        /// This method takes an <typeparamref name="TEntity"/> object, mark the object as <see cref="EntityState.Deleted"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entity"/> to be marked as deleted.</typeparam>
        /// <param name="entity">The <typeparamref name="TEntity"/> object to be deleted from the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Remove<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> objects, mark the objects as <see cref="EntityState.Deleted"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entities"/> to be marked as deleted.</typeparam>
        /// <param name="entities">The <typeparamref name="TEntity"/> objects to be deleted from the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Remove<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class;

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous save operation. The task result contains the number of state entries written to the database.
        /// </returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Contains all the repository methods.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the <see cref="DbContext"/>.</typeparam>
    public interface IRepository<TDbContext> : IRepository
    {
    }
}
