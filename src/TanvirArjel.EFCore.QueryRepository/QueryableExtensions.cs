// <copyright file="QueryableExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TanvirArjel.EFCore.GenericRepository
{
    /// <summary>
    /// Contains <see cref="Queryable"/> extension methods for paginated list.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Convert the <see cref="IQueryable{T}"/> into paginated list.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IQueryable"/>.</typeparam>
        /// <param name="source">The type to be extended.</param>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">Size of the pagiantion.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="PaginatedList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageIndex"/> is smaller than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageSize"/> is smaller than 1.</exception>
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "The value of pageIndex must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The value of pageSize must be greater than 0.");
            }

            long count = await source.LongCountAsync(cancellationToken);

            int skip = (pageIndex - 1) * pageSize;

            List<T> items = await source.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            PaginatedList<T> paginatedList = new PaginatedList<T>(items, count, pageIndex, pageSize);
            return paginatedList;
        }

        /// <summary>
        /// Convert the <see cref="IQueryable{T}"/> into paginated list.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IQueryable"/>.</typeparam>
        /// <param name="source">The type to be extended.</param>
        /// <param name="specification">An object of <see cref="PaginationSpecification{T}"/>.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="PaginatedList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="specification"/> is smaller than 1.</exception>
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            PaginationSpecification<T> specification,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            if (specification.PageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(specification.PageIndex), $"The value of {nameof(specification.PageIndex)} must be greater than 0.");
            }

            if (specification.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(specification.PageSize), $"The value of {nameof(specification.PageSize)} must be greater than 0.");
            }

            IQueryable<T> countSource = source;

            // modify the IQueryable using the specification's expression criteria
            if (specification.Conditions != null && specification.Conditions.Any())
            {
                foreach (Expression<Func<T, bool>> conditon in specification.Conditions)
                {
                    countSource = source.Where(conditon);
                }
            }

            long count = await countSource.LongCountAsync(cancellationToken);

            source = source.GetSpecifiedQuery(specification);
            List<T> items = await source.ToListAsync(cancellationToken);

            PaginatedList<T> paginatedList = new PaginatedList<T>(items, count, specification.PageIndex, specification.PageSize);
            return paginatedList;
        }
    }
}
