namespace WarehouseManagement.Api.Models;

public class ProductImage
{
    public int ProductImageId { get; set; }

    public int ProductId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public Product Product { get; set; } = null!;
}