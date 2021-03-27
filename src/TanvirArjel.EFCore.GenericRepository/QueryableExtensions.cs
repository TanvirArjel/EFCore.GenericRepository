// <copyright file="QueryableExtensions.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns>Returns <see cref="PaginatedList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageIndex"/> is smaller than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageSize"/> is smaller than 1.</exception>
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
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

            long count = await source.LongCountAsync();

            int skipe = (pageIndex - 1) * pageSize;

            List<T> items = await source.Skip(skipe).Take(pageSize).ToListAsync();

            PaginatedList<T> paginatedList = new PaginatedList<T>(items, count, pageIndex, pageSize);
            return paginatedList;
        }
    }
}
