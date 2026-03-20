using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class ProductsController(IProductRepository repo) : ControllerBase
{
   
    [HttpGet] // api/products
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type,string? sort)
    {
       return Ok (await repo.GetProductsAsync(brand, type, sort));
    }
    [HttpGet("{id:int}")] // api/products/3
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
       var product = await repo.GetProductByIdAsync(id);
         if (product == null) return NotFound();
         return Ok(product);
    }
    [HttpPost] // api/products
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
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
        repo.UpdateProduct(product);
        if(!await repo.SaveChangesAsync())
        {
            return BadRequest("Failed to update product");
        }
        return NoContent();
    }
     [HttpDelete("{id:int}")] // api/products/3
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        repo.DeleteProduct(product);
        if(!await repo.SaveChangesAsync())
        {
            return BadRequest("Failed to delete product");
        }
        return NoContent();
    }
   [HttpGet("brands")] // api/products/brands
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
       return Ok (await repo.GetBrandAsync());
    }
     [HttpGet("types")] // api/products/types   
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
       return Ok (await repo.GetTypesAsync());
    }
    // method for product already exists
    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }
}
