using System;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;
public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
{
   
    [HttpGet] // api/products
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
    {
       var spec = new ProductSpecification(specParams);
       return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize);
    }
    [HttpGet("{id:int}")] // api/products/3
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
       var product = await repo.GetByIdAsync(id);
         if (product == null) return NotFound();
         return Ok(product);
    }
    [HttpPost] // api/products
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
       if(await repo.SaveChangesAsync())
       {
           return CreatedAtAction(nameof(GetProduct), new {id = product.Id}, product);
       }
        return BadRequest("Failed to create product");
    }
    [HttpPut("{id:int}")] // api/products/3
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
       if(product.Id != id || !ProductExists(id))
              return BadRequest();
        repo.Update(product);
        if(!await repo.SaveChangesAsync())
        {
            return BadRequest("Failed to update product");
        }
        return NoContent();
    }
     [HttpDelete("{id:int}")] // api/products/3
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null) return NotFound();
        repo.Remove(product);
        if(!await repo.SaveChangesAsync())
        {
            return BadRequest("Failed to delete product");
        }
        return NoContent();
    }
   [HttpGet("brands")] // api/products/brands
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
      var spec = new BrandListSpecification();
      return Ok(await repo.ListAsync(spec));
    }
     [HttpGet("types")] // api/products/types   
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
      var spec = new TypeListSpecification();
      return Ok(await repo.ListAsync(spec));
    }
    // method for product already exists
    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }
}
