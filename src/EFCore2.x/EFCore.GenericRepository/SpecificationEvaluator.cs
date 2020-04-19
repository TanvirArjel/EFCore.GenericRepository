using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace EFCore.GenericRepository
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, Specification<T> specification) where T : class
        {
            if (inputQuery == null)
            {
                throw new ArgumentNullException(nameof(inputQuery));
            }

            var query = inputQuery;

            if (specification == null)
            {
                return query;
            }

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
            else if (!(string.IsNullOrWhiteSpace(specification.OrderByDynamic.ColumnName) || string.IsNullOrWhiteSpace(specification.OrderByDynamic.ColumnName)))
            {
                query = query.OrderBy(specification.OrderByDynamic.ColumnName + " " + specification.OrderByDynamic.SortDirection);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

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
    }
}
