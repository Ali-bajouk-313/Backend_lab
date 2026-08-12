using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(bool onlyAvailable = false);

    Task<Product?> GetByIdAsync(int id);

    Task<List<Product>> SearchAsync(
        string? name,
        string? supplier);

    Task<Product> CreateAsync(
        CreateProductRequest request);

    Task<(bool Success, string? Error, Product? Product)> UpdatePriceAsync(
        int id,
        decimal price);

    Task<(bool Success, string? Error, Product? Product)> UpdateQuantityAsync(
        int id,
        int quantity);

    Task<(bool Success, string? Error, Product? Product)> ArchiveAsync(
        int id);

    Task<(bool Success, string? Error, Product? Product)> AssignSupplierAsync(
        int id,
        int supplierId);
}