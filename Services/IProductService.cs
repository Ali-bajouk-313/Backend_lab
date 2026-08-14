using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public interface IProductService
{
    List<Product> GetAll(bool onlyAvailable);

    Product? GetById(int id);

    List<Product> Search(string? name, string? supplier);

    (bool Success, string? Error, Product? Product) Create(
        CreateProductRequest request);

    (bool Success, string? Error, Product? Product) UpdateQuantity(
        int id,
        int quantity);

    (bool Success, string? Error, Product? Product) UpdatePrice(
        int id,
        decimal price);

    (bool Success, string? Error, Product? Product) Archive(int id);

    (bool Success, string? Error, Product? Product) AssignSupplier(
        int productId,
        int supplierId);
}