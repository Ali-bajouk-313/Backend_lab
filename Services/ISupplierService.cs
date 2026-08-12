using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public interface ISupplierService
{
    Task<List<Supplier>> GetAllAsync();

    Task<Supplier?> GetByIdAsync(int id);

    Task<Supplier> CreateAsync(
        CreateSupplierRequest request);

    Task<bool> DeactivateAsync(int id);
}