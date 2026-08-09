using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public interface IProductService
{
    List<Product> GetAll(bool onlyAvailable);

    Product? GetById(Guid id);

    List<Product> Search(string? name, string? supplier);

    (bool Success, string? Error, Product? Product) Create(
        CreateProductRequest request);

    (bool Success, string? Error, Product? Product) UpdateQuantity(
        Guid id,
        int quantity);

    (bool Success, string? Error, Product? Product) UpdatePrice(
        Guid id,
        decimal price);

    (bool Success, string? Error, Product? Product) Archive(Guid id);

    (bool Success, string? Error, Product? Product) AssignSupplier(
        Guid productId,
        Guid supplierId);
}