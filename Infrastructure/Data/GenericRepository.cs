using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        context.Set<T>().Add(entity); //add the entity to the context
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
       var query = context.Set<T>().AsQueryable(); //get the queryable of the entity
       query = spec.ApplyCriteria(query); //apply the specification to the query
       return await query.CountAsync(); //return the count of the entities that match the specification
    }

    public bool Exists(int id)
    {
        return context.Set<T>().Any(e => e.Id == id); //check if the entity exists in the context   
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id); //find the entity by id
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await context.Set<T>().ToListAsync(); //return all entities of type T
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
       return await ApplySpecification(spec).ToListAsync(); //return the entities that match the specification
    }

    public  async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
      return await ApplySpecification(spec).ToListAsync(); //return the entities that match the specification
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity); //remove the entity from the context
    }
    

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0; 
    }

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified; //mark the entity as modified   
    }
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec); //apply the specification to the query
    }
    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), spec); //apply the specification to the query
    }
}
