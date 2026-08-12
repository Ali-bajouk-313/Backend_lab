using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.DatabaseFirst;
using WarehouseManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddDbContext<WarehouseDbFirstContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("WarehouseDbFirst")
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();