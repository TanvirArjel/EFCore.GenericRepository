// <copyright file="IRepository.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TanvirArjel.EFCore.GenericRepository.Services
{
    /// <summary>
    /// Contains all the repository methods.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets <see cref="IQueryable{T}"/> of the entity.
        /// </summary>
        IQueryable<TEntity> Entities { get; }

        /// <summary>
        /// This method returns <see cref="List{TEntity}"/> without any filter. Call only when you want pull all the data from the source.
        /// </summary>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="Task"/> of <see cref="List{TEntity}"/>.</returns>
        Task<List<TEntity>> GetEntityListAsync(bool asNoTracking = false);

        /// <summary>
        /// This method takes <paramref name="condition"/> as parameter and returns <see cref="List{TEntity}"/>.
        /// </summary>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<List<TEntity>> GetEntityListAsync(Expression<Func<TEntity, bool>> condition, bool asNoTracking = false);

        /// <summary>
        /// This method takes <paramref name="specification"/> as parameter and returns <see cref="List{TEntity}"/>.
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
        /// This method returns <see cref="List{TProjectedType}"/> without any filter.
        /// </summary>
        /// <typeparam name="TProjectedType">The type to which <typeparamref name="TEntity"/> will be projected.</typeparam>
        /// <param name="selectExpression">A <b>LINQ</b> select query.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<List<TProjectedType>> GetProjectedEntityListAsync<TProjectedType>(Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes <paramref name="condition"/> as parameter and returns <see cref="List{TProjectedType}"/>.
        /// </summary>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="selectExpression">The LINQ select query.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<List<TProjectedType>> GetProjectedEntityListAsync<TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes <paramref name="specification"/> and <paramref name="selectExpression"/> as parameters and
        /// returns <see cref="List{TProjectedType}"/>.
        /// </summary>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="selectExpression">The LINQ select query.</param>
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
        /// <param name="selectExpression">The LINQ select query.</param>
        /// <returns>Returns <see cref="Task"/> of <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<TProjectedType> GetProjectedEntityByIdAsync<TProjectedType>(
            object id,
            Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes <paramref name="condition"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="condition">The conditon on which entity will be returned.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Defualt value is false i.e trackig is enabled by default.
        /// </param>
        /// <returns>Returns <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> condition, bool asNoTracking = false);

        /// <summary>
        /// This method takes <paramref name="specification"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> object which contains all the conditions and criteria
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
        /// This method takes <paramref name="specification"/> and <paramref name="selectExpression"/> and returns <typeparamref name="TProjectedType"/>.
        /// </summary>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="specification">A <see cref="Specification{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="selectExpression">The select <b>LINQ</b> query.</param>
        /// <returns>Retuns <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <em>NULL</em>.</exception>
        Task<TProjectedType> GetProjectedEntityAsync<TProjectedType>(
            Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression);

        /// <summary>
        /// This method takes the <paramref name="condition"/> based on which existence of the entity will be checked
        /// and returns <see cref="Task"/> of <see cref="bool"/>.
        /// </summary>
        /// <param name="condition">The condition based on which the existence will checked.</param>
        /// <returns>Returns <see cref="bool"/>.</returns>
        Task<bool> IsEntityExistsAsync(Expression<Func<TEntity, bool>> condition);

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/> to be inserted and returns <see cref="Task"/>. Insertion in the database will be done after
        /// calling <em>IUnitOfWork.SaveChangesAsync()</em> method.
        /// </summary>
        /// <param name="entity">The entity to be inserted.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task InsertEntityAsync(TEntity entity);

        /// <summary>
        /// This method takes <em>IEnumerable</em> of entities to be inserted and returns <see cref="Task"/>.
        /// Insertion in the database will be done after calling <em>IUnitOfWork.SaveChangesAsync()</em> method.
        /// </summary>
        /// <param name="entities">The entities to be inserted.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task InsertEntitiesAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/> to be updated and returns <em>void</em>. Update in the database will be done after
        /// calling <em>IUnitOfWork.SaveChangesAsync()</em> method.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        void UpdateEntity(TEntity entity);

        /// <summary>
        /// This method takes <em>IEnumerable</em> of entities to be updated and returns <em>void</em>. Update in the database will be done after
        /// calling <em>IUnitOfWork.SaveChangesAsync()</em> method.
        /// </summary>
        /// <param name="entities">The entities to be updated.</param>
        void UpdateEntities(IEnumerable<TEntity> entities);

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/>, delete the entity and returns <em>void</em>. Deletion in the database will be done after
        /// calling <em>IUnitOfWork.SaveChangesAsync()</em> method.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        void DeleteEntity(TEntity entity);

        /// <summary>
        /// This method takes <em>IEnumerable</em> of entities, delete those entities and returns <em>void</em>. Deletion in the database will
        /// be done after calling <em>IUnitOfWork.SaveChangesAsync()</em> method.
        /// </summary>
        /// <param name="entities">The list of entities to be deleted.</param>
        void DeleteEntities(IEnumerable<TEntity> entities);

        /// <summary>
        /// This method takes one or more <paramref name="conditions"/> and returns the count in <see cref="int"/> type.
        /// </summary>
        /// <param name="conditions">The condition or conditions based on which count will be done.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
        Task<int> GetCountAsync(params Expression<Func<TEntity, bool>>[] conditions);

        /// <summary>
        /// This method takes one or more <paramref name="conditions"/> and returns the count in <see cref="long"/> type.
        /// </summary>
        /// <param name="conditions">The condition or conditions based on which count will be done.</param>
        /// <returns>Retuns <see cref="Task"/> of <see cref="long"/>.</returns>
        Task<long> GetLongCountAsync(params Expression<Func<TEntity, bool>>[] conditions);
    }
}
