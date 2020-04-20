// <copyright file="Specification.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.GenericRepository
{
    /// <summary>
    /// This object hold the query specifications.
    /// </summary>
    /// <typeparam name="T">The database entity.</typeparam>
    public class Specification<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets the <see cref="Expression{TDelegate}"/> list you want to pass with your EF Core query.
        /// </summary>
        public List<Expression<Func<T, bool>>> Conditions { get; set; } = new List<Expression<Func<T, bool>>>();

        /// <summary>
        /// Gets or sets the navigation entities to be eager loadded with EF Core query.
        /// </summary>
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }

        /// <summary>
        /// Gets or add the navigation entities to be eager loadded with EF Core query in string format.
        /// </summary>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the <see cref="Func{T, TResult}"/> to order by your query.
        /// </summary>
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }

        /// <summary>
        /// Gets or sets dynmic order by option in string format.
        /// </summary>
        public (string ColumnName, string SortDirection) OrderByDynamic { get; set; }

        /// <summary>
        /// Gets or sets the value of number of entity you want to skip in your query.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets the value of number of entity you want to take in your query.
        /// </summary>
        public int? Take { get; set; }
    }
}
