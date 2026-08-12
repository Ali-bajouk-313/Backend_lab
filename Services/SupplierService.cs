using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public class SupplierService : ISupplierService
{
    private readonly WarehouseDBContext _context;

    public SupplierService(WarehouseDBContext context)
    {
        _context = context;
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.SupplierId == id);
    }

    public async Task<Supplier> CreateAsync(
        CreateSupplierRequest request)
    {
        bool exists = await _context.Suppliers
            .AnyAsync(s =>
                s.Name.ToLower() == request.Name.ToLower());

        if (exists)
        {
            throw new InvalidOperationException(
                "Supplier already exists.");
        }

        var supplier = new Supplier
        {
            Name = request.Name,
            Country = request.Country,
            ContactEmail = request.ContactEmail,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };

        _context.Suppliers.Add(supplier);

        await _context.SaveChangesAsync();

        return supplier;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.SupplierId == id);

        if (supplier == null)
        {
            return false;
        }

        supplier.IsActive = false;

        await _context.SaveChangesAsync();

        return true;
    }
}