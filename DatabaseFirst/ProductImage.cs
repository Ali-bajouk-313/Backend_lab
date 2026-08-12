using System;

namespace WarehouseManagement.Api.DatabaseFirst;

public class ProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public virtual Product Product { get; set; } = null!;
}