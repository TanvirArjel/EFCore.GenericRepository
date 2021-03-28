// <copyright file="SpecificationEvaluator.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TanvirArjel.EFCore.GenericRepository
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, Specification<T> specification)
            where T : class
        {
            IQueryable<T> query = GetSpecifiedQuery(inputQuery, (SpecificationBase<T>)specification);

            // Apply paging if enabled
            if (specification.Skip != null)
            {
                query = query.Skip((int)specification.Skip);
            }

            if (specification.Take != null)
            {
                query = query.Take((int)specification.Take);
            }

            return query;
        }

        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, PaginationSpecification<T> specification)
            where T : class
        {
            if (inputQuery == null)
            {
                throw new ArgumentNullException(nameof(inputQuery));
            }

            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            if (specification.PageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(specification.PageIndex), "The value of pageIndex must be greater than 0.");
            }

            if (specification.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(specification.PageSize), "The value of pageSize must be greater than 0.");
            }

            IQueryable<T> query = GetSpecifiedQuery(inputQuery, (SpecificationBase<T>)specification);

            // Apply paging if enabled
            int skip = (specification.PageIndex - 1) * specification.PageSize;

            query = query.Skip(skip).Take(specification.PageSize);

            return query;
        }

        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, SpecificationBase<T> specification)
            where T : class
        {
            if (inputQuery == null)
            {
                throw new ArgumentNullException(nameof(inputQuery));
            }

            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            IQueryable<T> query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            if (specification.Conditions != null && specification.Conditions.Any())
            {
                foreach (Expression<Func<T, bool>> specificationCondition in specification.Conditions)
                {
                    query = query.Where(specificationCondition);
                }
            }

            // Includes all expression-based includes
            if (specification.Includes != null)
            {
                query = specification.Includes(query);
            }

            // Include any string-based include statements
            if (specification.IncludeStrings != null && specification.IncludeStrings.Any())
            {
                query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            }

            // Apply ordering if expressions are set
            if (specification.OrderBy != null)
            {
                query = specification.OrderBy(query);
            }
            else if (!string.IsNullOrWhiteSpace(specification.OrderByDynamic.ColumnName) && !string.IsNullOrWhiteSpace(specification.OrderByDynamic.ColumnName))
            {
                query = query.OrderBy(specification.OrderByDynamic.ColumnName + " " + specification.OrderByDynamic.SortDirection);
            }

            return query;
        }
    }
}
