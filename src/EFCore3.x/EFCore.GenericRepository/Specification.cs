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
    public class Specification<T>
        where T : class
    {
        public List<Expression<Func<T, bool>>> Conditions { get; set; } = new List<Expression<Func<T, bool>>>();

        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }

        public List<string> IncludeStrings { get; } = new List<string>();

        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }

        public (string ColumnName, string SortDirection) OrderByDynamic { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
