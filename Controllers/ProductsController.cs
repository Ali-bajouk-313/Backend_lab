using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.DatabaseFirst;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
private readonly WarehouseDbFirstContext _context;


public ProductsController(WarehouseDbFirstContext context)
{
    _context = context;
}

[HttpGet]
public async Task<IActionResult> GetAll()
{
    var products = await _context.Products
        .Include(p => p.Supplier)
        .Include(p => p.ProductImages)
        .ToListAsync();

    return Ok(products);
}

[HttpGet("{id:int}")]
public async Task<IActionResult> GetById(int id)
{
    var product = await _context.Products
        .Include(p => p.Supplier)
        .Include(p => p.ProductImages)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (product == null)
    {
        return NotFound(new
        {
            message = "Product not found."
        });
    }

    return Ok(product);
}

[HttpGet("by-supplier")]
public async Task<IActionResult> GetProductsBySupplier(
    [FromQuery] string supplierName,
    [FromQuery] string sort = "asc")
{
    if (string.IsNullOrWhiteSpace(supplierName))
    {
        return BadRequest(new
        {
            message = "Supplier name is required."
        });
    }

    if (sort.ToLower() != "asc" &&
        sort.ToLower() != "desc")
    {
        return BadRequest(new
        {
            message = "Sort must be either 'asc' or 'desc'."
        });
    }

    var query = _context.Products
        .Include(p => p.Supplier)
        .Where(p => p.Supplier != null && p.Supplier.Name == supplierName);

    if (sort.ToLower() == "desc")
    {
        query = query.OrderByDescending(p => p.CreatedAt);
    }
    else
    {
        query = query.OrderBy(p => p.CreatedAt);
    }

    var products = await query.ToListAsync();

    return Ok(products);
}

[HttpGet("group-by-expiry-year")]
public async Task<IActionResult> GroupByExpiryYear()
{
    var result = await _context.Products
        .Where(p => p.ExpiryDate.HasValue)
        .GroupBy(p => p.ExpiryDate!.Value.Year)
        .Select(group => new
        {
            ExpiryYear = group.Key,
            ProductCount = group.Count(),
            Products = group.ToList()
        })
        .OrderBy(x => x.ExpiryYear)
        .ToListAsync();

    return Ok(result);
}

[HttpGet("group-by-expiry-year-country")]
public async Task<IActionResult> GroupByExpiryYearAndSupplierCountry()
{
    var result = await _context.Products
        .Include(p => p.Supplier)
        .Where(p => p.ExpiryDate.HasValue && p.Supplier != null)
        .GroupBy(p => new
        {
            ExpiryYear = p.ExpiryDate!.Value.Year,
            Country = p.Supplier!.Country
        })
        .Select(group => new
        {
            ExpiryYear = group.Key.ExpiryYear,
            Country = group.Key.Country,
            ProductCount = group.Count(),
            Products = group.ToList()
        })
        .OrderBy(x => x.ExpiryYear)
        .ThenBy(x => x.Country)
        .ToListAsync();

    return Ok(result);
}

[HttpGet("count")]
public async Task<IActionResult> GetProductCount()
{
    var count = await _context.Products.CountAsync();

    return Ok(new
    {
        totalProducts = count
    });
}



[HttpGet("pagination")]
public async Task<IActionResult> GetProductsPaginated(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
{
    if (pageNumber < 1)
    {
        return BadRequest(new
        {
            message = "pageNumber must be greater than 0."
        });
    }

    if (pageSize < 1)
    {
        return BadRequest(new
        {
            message = "pageSize must be greater than 0."
        });
    }

    var totalProducts = await _context.Products.CountAsync();

    var totalPages = (int)Math.Ceiling(
        (double)totalProducts / pageSize
    );

    var products = await _context.Products
        .Include(p => p.Supplier)
        .OrderBy(p => p.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(new
    {
        pageNumber,
        pageSize,
        totalProducts,
        totalPages,
        products
    });
}

[HttpGet("debug-db")]
public async Task<IActionResult> DebugDatabase()
{
    var database = await _context.Database
        .SqlQueryRaw<string>("SELECT current_database() AS \"Value\"")
        .FirstAsync();

    var schema = await _context.Database
        .SqlQueryRaw<string>("SELECT current_schema() AS \"Value\"")
        .FirstAsync();

    return Ok(new
    {
        database,
        schema
    });
}

}
