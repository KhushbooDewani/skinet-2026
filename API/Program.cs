using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductRepository, ProductRepository>(); //scoped used because we want to create a new instance of the repository for each request
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //register the generic repository
var app = builder.Build();

app.MapControllers();//map controllers to endpoints

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB.");
}
app.Run();

