// <copyright file="IRepository.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Contains all the repository methods.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets <see cref="IQueryable{T}"/> of the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Returns <see cref="IQueryable{T}"/>.</returns>
        IQueryable<TEntity> GetQueryable<TEntity>()
            where TEntity : class;

        /// <summary>
        /// This method returns <see cref="List{T}"/> without any filter. Call only when you want to pull all the data from the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task"/> of <see cref="List{T}"/>.</returns>
        Task<List<TEntity>> GetEntityListAsync<TEntity>(bool asNoTracking = false)
            where TEntity : class;

        /// <summary>
        /// This method takes a <see cref="Expression{Func}"/> as parameter and returns <see cref="List{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<List<TEntity>> GetEntityListAsync<TEntity>(Expression<Func<TEntity, bool>> condition, bool asNoTracking = false)
            where TEntity : class;

        /// <summary>
        /// This method takes an object of <see cref="Specification{TEntity}"/> as parameter and returns <see cref="List{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> <see cref="object"/> which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<List<TEntity>> GetEntityListAsync<TEntity>(Specification<TEntity> specification, bool asNoTracking = false)
            where TEntity : class;

        /// <summary>
        /// This method returns <see cref="List{TProjectedType}"/> without any filter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The type to which <typeparamref name="TEntity"/> will be projected.</typeparam>
        /// <param name="selectExpression">A <b>LINQ</b> select query.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<List<TProjectedType>> GetProjectedEntityListAsync<TEntity, TProjectedType>(
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <see cref="List{TProjectedType}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<List<TProjectedType>> GetProjectedEntityListAsync<TEntity, TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TEntity : class;

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="Specification{T}"/> and <paramref name="selectExpression"/> as parameters and
        /// returns <see cref="List{TProjectedType}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <returns>Return <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<List<TProjectedType>> GetProjectedEntityListAsync<TEntity, TProjectedType>(
            Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TEntity : class;

        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the entity
        /// if found otherwise <em>NULL</em>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetEntityByIdAsync<TEntity>(object id, bool asNoTracking = false)
            where TEntity : class;

        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the specified projected entity
        /// if found otherwise null.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <returns>Returns <see cref="Task"/> of <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<TProjectedType> GetProjectedEntityByIdAsync<TEntity, TProjectedType>(
            object id,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The conditon on which entity will be returned.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetEntityAsync<TEntity>(Expression<Func<TEntity, bool>> condition, bool asNoTracking = false)
            where TEntity : class;

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="Specification{T}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetEntityAsync<TEntity>(Specification<TEntity> specification, bool asNoTracking = false)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="condition">The conditon on which entity will be returned.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <returns>Retuns <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<TProjectedType> GetProjectedEntityAsync<TEntity, TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TEntity : class;

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="Specification{T}"/> and a <see cref="System.Linq"/> select  query
        /// and returns <typeparamref name="TProjectedType"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The type of the projected entity.</typeparam>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select  query.</param>
        /// <returns>Retuns <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<TProjectedType> GetProjectedEntityAsync<TEntity, TProjectedType>(
            Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TEntity : class;

        /// <summary>
        /// This method takes a predicate based on which existence of the entity will be determined
        /// and returns <see cref="Task"/> of <see cref="bool"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition based on which the existence will checked.</param>
        /// <returns>Returns <see cref="bool"/>.</returns>
        Task<bool> IsEntityExistsAsync<TEntity>(Expression<Func<TEntity, bool>> condition)
            where TEntity : class;

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/> to be inserted and returns <see cref="Task"/>.
        /// Insertion in the database will be done after calling <see cref="SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task InsertEntityAsync<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// This method takes <em>IEnumerable</em> of entities to be inserted and returns <see cref="Task"/>.
        /// Insertion in the database will be affected after calling <see cref="SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities to be inserted.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task InsertEntitiesAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class;

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/> to be updated and returns <see cref="void"/>.
        /// Update in the database will be affected after calling <see cref="SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be updated.</param>
        void UpdateEntity<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> of entities to be updated and returns <see cref="void"/>.
        /// Update in the database will be affected after calling <see cref="SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities to be updated.</param>
        void UpdateEntities<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class;

        /// <summary>
        /// This method takes an entity of type <typeparamref name="TEntity"/>, delete the entity and returns <see cref="void"/>.
        /// Deletion in the database will be affected after calling <see cref="SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be deleted.</param>
        void DeleteEntity<TEntity>(TEntity entity)
            where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{T}"/> of entities, delete those entities and returns <see cref="void"/>.
        /// Deletion in the database will be affected after calling <see cref="SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The list of entities to be deleted.</param>
        void DeleteEntities<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class;

        /// <summary>
        /// This method returns all count in <see cref="int"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
        Task<int> GetCountAsync<TEntity>()
            where TEntity : class;

        /// <summary>
        /// This method takes one or more <em>predicates</em> and returns the count in <see cref="int"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions">The condition or conditions based on which count will be done.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
        Task<int> GetCountAsync<TEntity>(params Expression<Func<TEntity, bool>>[] conditions)
            where TEntity : class;

        /// <summary>
        /// This method returns all count in <see cref="long"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Retuns <see cref="Task"/> of <see cref="long"/>.</returns>
        Task<long> GetLongCountAsync<TEntity>()
            where TEntity : class;

        /// <summary>
        /// This method takes one or more <em>predicates</em> and returns the count in <see cref="long"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions">The condition or conditions based on which count will be done.</param>
        /// <returns>Retuns <see cref="Task"/> of <see cref="long"/>.</returns>
        Task<long> GetLongCountAsync<TEntity>(params Expression<Func<TEntity, bool>>[] conditions)
            where TEntity : class;

        // Context level members

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
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
