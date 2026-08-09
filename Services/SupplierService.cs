using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public class SupplierService : ISupplierService
{
    public List<Supplier> GetAll()
    {
        return FakeWarehouseStore.Suppliers;
    }

    public Supplier? GetById(Guid id)
    {
        return FakeWarehouseStore.Suppliers
            .FirstOrDefault(s => s.Id == id);
    }

    public (bool Success, string? Error, Supplier? Supplier) Create(
        CreateSupplierRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return (false, "Supplier name is required.", null);
        }

        bool exists = FakeWarehouseStore.Suppliers.Any(s =>
            s.Name.Equals(
                request.Name,
                StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            return (false, "Supplier already exists.", null);
        }

        Supplier supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Country = request.Country,
            ContactEmail = request.ContactEmail,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };

        FakeWarehouseStore.Suppliers.Add(supplier);

        return (true, null, supplier);
    }

    public (bool Success, string? Error, Supplier? Supplier) Deactivate(
        Guid id)
    {
        Supplier? supplier = GetById(id);

        if (supplier == null)
        {
            return (false, "Supplier not found.", null);
        }

        if (!supplier.IsActive)
        {
            return (false, "Supplier is already inactive.", null);
        }

        supplier.IsActive = false;

        return (true, null, supplier);
    }
}