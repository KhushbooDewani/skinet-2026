using System;
using System.Linq.Expressions;
using Microsoft.VisualBasic;

namespace Core.Interfaces;

public interface ISpecification<T>
{
   Expression<Func<T, bool>>? Criteria { get; } // criteria is a lambda expression that defines the condition for filtering the data
   Expression<Func<T,object>>? OrderBy { get; } // orderby is a lambda expression that defines the condition for sorting the data in ascending order
   Expression<Func<T,object>>? OrderByDescending { get; } // orderbydescending
   bool IsDistinct { get; } // isdistinct is a boolean that defines whether the query should return distinct results
   int Take { get; } // take is an integer that defines the number of records to return
   int Skip { get; } // skip is an integer that defines the number of records to skip
   bool IsPagingEnabled { get; } // ispagingenabled is a boolean that defines whether the query should return paginated results  
   IQueryable<T> ApplyCriteria(IQueryable<T> query); // applycriteria is a method that applies the criteria to the query
} 
// used because we want to return a different type of data than the original type
public interface ISpecification<T, TResult> : ISpecification<T>
{
   Expression<Func<T,TResult>>? Select {get;}
}