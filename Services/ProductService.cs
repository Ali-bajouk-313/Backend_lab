using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public class ProductService : IProductService
{
    private readonly WarehouseDBContext _context;

    public ProductService(WarehouseDBContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync(
        bool onlyAvailable = false)
    {
        var query = _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Images)
            .Where(p => !p.IsArchived);

        if (onlyAvailable)
        {
            query = query.Where(p => p.QuantityInStock > 0);
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> SearchAsync(
        string? name,
        string? supplier)
    {
        var query = _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Images)
            .Where(p => !p.IsArchived)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p =>
                p.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(supplier))
        {
            query = query.Where(p =>
                p.Supplier != null &&
                p.Supplier.Name.Contains(supplier));
        }

        return await query
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(
        CreateProductRequest request)
    {
        bool skuExists = await _context.Products
            .AnyAsync(p => p.SKU == request.SKU);

        if (skuExists)
        {
            throw new InvalidOperationException(
                "SKU already exists.");
        }

        var product = new Product
        {
            Name = request.Name,
            SKU = request.SKU,
            Description = request.Description,
            Price = request.Price,
            QuantityInStock = request.QuantityInStock,
            SupplierName = request.SupplierName,
            ExpiryDate = request.ExpiryDate,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<(bool Success, string? Error, Product? Product)>
        UpdatePriceAsync(int id, decimal price)
    {
        if (price <= 0)
        {
            return (false, "Price must be greater than zero.", null);
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return (false, "Product not found.", null);
        }

        product.Price = price;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return (true, null, product);
    }

    public async Task<(bool Success, string? Error, Product? Product)>
        UpdateQuantityAsync(int id, int quantity)
    {
        if (quantity < 0)
        {
            return (false, "Quantity cannot be negative.", null);
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return (false, "Product not found.", null);
        }

        product.QuantityInStock = quantity;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return (true, null, product);
    }

    public async Task<(bool Success, string? Error, Product? Product)>
        ArchiveAsync(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return (false, "Product not found.", null);
        }

        product.IsArchived = true;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return (true, null, product);
    }

    public async Task<(bool Success, string? Error, Product? Product)>
        AssignSupplierAsync(int id, int supplierId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return (false, "Product not found.", null);
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.SupplierId == supplierId);

        if (supplier == null)
        {
            return (false, "Supplier not found.", null);
        }

        if (!supplier.IsActive)
        {
            return (false, "Supplier is not active.", null);
        }

        product.SupplierId = supplierId;
        product.SupplierName = supplier.Name;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return (true, null, product);
    }
}