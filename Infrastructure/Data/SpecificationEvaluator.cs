using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T>  where T : BaseEntity
{
    // method to evaluate the specification and return the filtered query
   public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
   {
      var query = inputQuery;
      if (spec.Criteria != null)
      {
         query = query.Where(spec.Criteria); // x => x.Brand == brands
      }
      if(spec.OrderBy != null)
      {
         query = query.OrderBy(spec.OrderBy);
      }
      if(spec.OrderByDescending != null)
      {
         query = query.OrderByDescending(spec.OrderByDescending);
      }
      if(spec.IsDistinct)
      {
         query = query.Distinct();
      }
      if(spec.IsPagingEnabled)
      {
         query = query.Skip(spec.Skip).Take(spec.Take);
      }
      return query;
   }
    public static IQueryable<TResult> GetQuery<TSpecification, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> spec)
      
   {
      var query = inputQuery;
      if (spec.Criteria != null)
      {
         query = query.Where(spec.Criteria); // x => x.Brand == brands
      }
      if(spec.OrderBy != null)
      {
         query = query.OrderBy(spec.OrderBy);
      }
      if(spec.OrderByDescending != null)
      {
         query = query.OrderByDescending(spec.OrderByDescending);
      }
      var selectQuery = query as IQueryable<TResult>;
     if(spec.Select != null)
     {
      // This line transforms the query from 'Product' to 'string'
      selectQuery = query.Select(spec.Select);
     }
     if(spec.IsDistinct)
     {
        selectQuery = selectQuery?.Distinct();
     }
     if(spec.IsPagingEnabled)
      {
         selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
      }
      return selectQuery ?? query.Cast<TResult>();
   }
}
