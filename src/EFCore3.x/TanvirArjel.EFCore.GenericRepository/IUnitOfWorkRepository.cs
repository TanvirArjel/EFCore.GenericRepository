// <copyright file="IUnitOfWorkRepository.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Contains all the repository methods.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IUnitOfWorkRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets <see cref="IQueryable{T}"/> of the entity.
        /// </summary>
        IQueryable<TEntity> Entities { get; }

        /// <summary>
        /// This method returns <see cref="Task{List}"/> without any filter. Call only when you want to pull all the data from the source.
        /// </summary>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task"/> of <see cref="List{TEntity}"/>.</returns>
        Task<List<TEntity>> GetEntityListAsync(bool asNoTracking = false);

        /// <summary>
        /// This method takes an <see cref="Expression{Func}"/> as parameter and returns <see cref="Task"/> of <see cref="List{TEntity}"/>.
        /// </summary>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task"/> of <see cref="List{TEntity}"/>.</returns>
        Task<List<TEntity>> GetEntityListAsync(Expression<Func<TEntity, bool>> condition, bool asNoTracking = false);

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="Specification{TEntity}"/> as parameter
        /// and returns <see cref="Task"/> of <see cref="List{TEntity}"/>.
        /// </summary>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<List<TEntity>> GetEntityListAsync(Specification<TEntity> specification, bool asNoTracking = false);

        /// <summary>
        /// This method returns <see cref="Task"/> of <see cref="List{TProjectedType}"/> without any filter.
        /// </summary>
        /// <typeparam name="TProjectedType">The type to which <typeparamref name="TEntity"/> will be projected.</typeparam>
        /// <param name="selectExpression">A <see cref="System.Linq"/> select query.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="List{TProjectedType}"/>.</returns>
        Task<List<TProjectedType>> GetProjectedEntityListAsync<TProjectedType>(Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <see cref="Task"/> of <see cref="List{TProjectedType}"/>.
        /// </summary>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<List<TProjectedType>> GetProjectedEntityListAsync<TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="Specification{TTEntity}"/> and <paramref name="selectExpression"/>
        /// as parameters and returns <see cref="List{TProjectedType}"/>.
        /// </summary>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> <see cref="object"/> which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <returns>Return <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<List<TProjectedType>> GetProjectedEntityListAsync<TProjectedType>(
            Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the entity
        /// if found otherwise <em>NULL</em>.
        /// </summary>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetEntityByIdAsync(object id, bool asNoTracking = false);

        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the specified projected entity
        /// if found otherwise null.
        /// </summary>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <returns>Returns <see cref="Task"/> of <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> value is <em>NULL</em>.</exception>
        Task<TProjectedType> GetProjectedEntityByIdAsync<TProjectedType>(
            object id,
            Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes <see cref="Expression{Predicate}"/> as parameter and returns <see cref="Task"/> of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="condition">The conditon on which entity will be returned.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task"/> of <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> condition, bool asNoTracking = false);

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="Specification{TEntity}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> <see cref="object"/> which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetEntityAsync(Specification<TEntity> specification, bool asNoTracking = false);

        /// <summary>
        /// This method takes <paramref name="condition"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="condition">The conditon on which entity will be returned.</param>
        /// <param name="selectExpression">The select <b>LINQ</b> query.</param>
        /// <returns>Retuns <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<TProjectedType> GetProjectedEntityAsync<TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="Specification{TEntity}"/> and <paramref name="selectExpression"/>
        /// and returns an <see cref="object"/> of type <typeparamref name="TProjectedType"/> if found.
        /// </summary>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="selectExpression">The select <b>LINQ</b> query.</param>
        /// <returns>Retuns <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> value is <em>NULL</em>.</exception>
        Task<TProjectedType> GetProjectedEntityAsync<TProjectedType>(
            Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes <see cref="Expression{Predicate}"/> based on which existence of the entity will be checked
        /// and returns <see cref="Task"/> of <see cref="bool"/>.
        /// </summary>
        /// <param name="condition">The condition based on which the existence will checked.</param>
        /// <returns>Returns <see cref="bool"/>.</returns>
        Task<bool> IsEntityExistsAsync([NotNull] Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// This method takes an <see cref="object"/> of type <typeparamref name="TEntity"/> to be inserted and returns <see cref="Task"/>.
        /// Insertion in the database will be affected after calling <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task InsertEntityAsync([NotNull] TEntity entity);

        /// <summary>
        /// This method takes <em>IEnumerable</em> of entities to be inserted and returns <see cref="Task"/>.
        /// Insertion in the database will be affected after calling <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <param name="entities">The entities to be inserted.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task InsertEntitiesAsync([NotNull] IEnumerable<TEntity> entities);

        /// <summary>
        /// This method takes an <see cref="object"/> of type <typeparamref name="TEntity"/> to be updated and returns <see cref="void"/>.
        /// Update in the database will be affected after calling <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        void UpdateEntity([NotNull] TEntity entity);

        /// <summary>
        /// This method takes <em>IEnumerable</em> of entities to be updated and returns <see cref="void"/>.
        /// Update in the database will be affected after calling <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <param name="entities">The entities to be updated.</param>
        void UpdateEntities([NotNull] IEnumerable<TEntity> entities);

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/>, delete the entity and returns <see cref="void"/>.
        /// Deletion in the database will be affected after calling <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        void DeleteEntity([NotNull] TEntity entity);

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> of entities, delete those entities and returns <see cref="void"/>.
        /// Deletion in the database will be affected after calling <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)"/> method.
        /// </summary>
        /// <param name="entities">The list of entities to be deleted.</param>
        void DeleteEntities([NotNull] IEnumerable<TEntity> entities);

        /// <summary>
        /// This method returns all count in <see cref="int"/> type.
        /// </summary>
        /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// This method takes one or more <see cref="Expression{Predicate}"/> and returns the count in <see cref="int"/> type.
        /// </summary>
        /// <param name="conditions">The condition or conditions based on which count will be done.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
        Task<int> GetCountAsync([NotNull] params Expression<Func<TEntity, bool>>[] conditions);

        /// <summary>
        /// This method returns all long count in <see cref="long"/> type.
        /// </summary>
        /// <returns>Returns <see cref="Task"/> of <see cref="long"/>.</returns>
        Task<long> GetLongCountAsync();

        /// <summary>
        /// This method takes one or more <see cref="Expression{Predicate}"/> and returns the count in <see cref="long"/> type.
        /// </summary>
        /// <param name="conditions">The condition or conditions based on which count will be done.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="long"/>.</returns>
        Task<long> GetLongCountAsync([NotNull] params Expression<Func<TEntity, bool>>[] conditions);
    }
}
