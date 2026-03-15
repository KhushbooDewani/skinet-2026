using System;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class ProductsController : ControllerBase
{
    private readonly StoreContext context;

    public ProductsController(StoreContext context)
    {
        this.context = context;
    }

    [HttpGet] // api/products
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
       return await context.Products.ToListAsync();
    }
    [HttpGet("{id:int}")] // api/products/3
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
       var product = await context.Products.FindAsync(id);
         if (product == null) return NotFound();
         return Ok(product);
    }
    [HttpPost] // api/products
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }
    [HttpPut("{id:int}")] // api/products/3
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
       if(product.Id != id || !ProductExists(id))
              return BadRequest();
        context.Entry(product).State = EntityState.Modified; // mark the entity as modified
        await context.SaveChangesAsync();
        return NoContent();
    }
     [HttpDelete("{id:int}")] // api/products/3
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return NoContent();
    }

    // method for product already exists
    private bool ProductExists(int id)
    {
        return context.Products.Any(e => e.Id == id);
    }
}
