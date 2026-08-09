using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public interface ISupplierService
{
    List<Supplier> GetAll();

    Supplier? GetById(Guid id);

    (bool Success, string? Error, Supplier? Supplier) Create(
        CreateSupplierRequest request);

    (bool Success, string? Error, Supplier? Supplier) Deactivate(
        Guid id);
}