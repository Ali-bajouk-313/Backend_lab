using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.Services;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ISupplierService _supplierService;
    private readonly IWebHostEnvironment _environment;

    public ProductsController(
        IProductService productService,
        ISupplierService supplierService,
        IWebHostEnvironment environment)
    {
        _productService = productService;
        _supplierService = supplierService;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] bool onlyAvailable = false)
    {
        List<Product> products =
            _productService.GetAll(onlyAvailable);

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(
        [FromRoute] Guid id)
    {
        Product? product = _productService.GetById(id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found."
            });
        }

        return Ok(product);
    }

    [HttpGet("search")]
    public IActionResult Search(
        [FromQuery] string? name,
        [FromQuery] string? supplier)
    {
        if (string.IsNullOrWhiteSpace(name) &&
            string.IsNullOrWhiteSpace(supplier))
        {
            return BadRequest(new
            {
                message = "At least one search parameter is required."
            });
        }

        List<Product> products =
            _productService.Search(name, supplier);

        return Ok(products);
    }

    // POST /api/products
    [HttpPost]
    public IActionResult Create(
        [FromBody] CreateProductRequest request)
    {
        var result = _productService.Create(request);

        if (!result.Success)
        {
            return BadRequest(new
            {
                message = result.Error
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Product!.Id },
            result.Product);
    }

    // POST /api/products/{id}/quantity
    [HttpPost("{id:guid}/quantity")]
    public IActionResult UpdateQuantity(
        [FromRoute] Guid id,
        [FromBody] UpdateProductQuantityRequest request)
    {
        var result = _productService.UpdateQuantity(
            id,
            request.QuantityInStock);

        if (!result.Success)
        {
            if (result.Error == "Product not found.")
            {
                return NotFound(new
                {
                    message = result.Error
                });
            }

            return BadRequest(new
            {
                message = result.Error
            });
        }

        return Ok(result.Product);
    }

    // POST /api/products/{id}/price
    [HttpPost("{id:guid}/price")]
    public IActionResult UpdatePrice(
        [FromRoute] Guid id,
        [FromBody] UpdateProductPriceRequest request)
    {
        var result = _productService.UpdatePrice(
            id,
            request.Price);

        if (!result.Success)
        {
            if (result.Error == "Product not found.")
            {
                return NotFound(new
                {
                    message = result.Error
                });
            }

            return BadRequest(new
            {
                message = result.Error
            });
        }

        return Ok(result.Product);
    }

    // POST /api/products/{id}/image
    [HttpPost("{id:guid}/image")]
    public async Task<IActionResult> UploadImage(
        [FromRoute] Guid id,
        [FromForm] IFormFile file)
    {
        Product? product = _productService.GetById(id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found."
            });
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest(new
            {
                message = "Image file is required."
            });
        }

        if (file.Length > 2 * 1024 * 1024)
        {
            return BadRequest(new
            {
                message = "Maximum file size is 2 MB."
            });
        }

        string extension =
            Path.GetExtension(file.FileName).ToLowerInvariant();

        string[] allowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new
            {
                message = "Only JPG and PNG images are allowed."
            });
        }

        string uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads");

        Directory.CreateDirectory(uploadsFolder);

        string fileName =
            $"{Guid.NewGuid()}{extension}";

        string filePath =
            Path.Combine(uploadsFolder, fileName);

        using FileStream stream =
            new FileStream(
                filePath,
                FileMode.Create);

        await file.CopyToAsync(stream);

        ProductImage image = new ProductImage
        {
            ProductId = id,
            FileName = file.FileName,
            FilePath = $"/uploads/{fileName}"
        };

        product.Images.Add(image);
        product.LastUpdatedAt = DateTime.UtcNow;

        return Ok(image);
    }

    // DELETE /api/products/{id}
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(
        [FromRoute] Guid id)
    {
        var result = _productService.Archive(id);

        if (!result.Success)
        {
            return NotFound(new
            {
                message = result.Error
            });
        }

        return Ok(new
        {
            message = "Product archived successfully.",
            product = result.Product
        });
    }

    // GET /api/products/server-time
    [HttpGet("server-time")]
    public IActionResult GetServerTime(
        [FromHeader(Name = "Accept-Language")]
        string? language)
    {
        string cultureName = language?.ToLowerInvariant() switch
        {
            "fr-fr" => "fr-FR",
            "ar-lb" => "ar-LB",
            _ => "en-US"
        };

        CultureInfo culture =
            CultureInfo.GetCultureInfo(cultureName);

        string formattedDate =
            DateTime.Now.ToString(
                "F",
                culture);

        return Ok(new
        {
            language = cultureName,
            serverTime = formattedDate
        });
    }

    // POST /api/products/{id}/assign-supplier/{supplierId}
    [HttpPost("{id:guid}/assign-supplier/{supplierId:guid}")]
    public IActionResult AssignSupplier(
        [FromRoute] Guid id,
        [FromRoute] Guid supplierId)
    {
        var result = _productService.AssignSupplier(
            id,
            supplierId);

        if (!result.Success)
        {
            if (result.Error == "Product not found.")
            {
                return NotFound(new
                {
                    message = result.Error
                });
            }

            if (result.Error == "Supplier not found.")
            {
                return NotFound(new
                {
                    message = result.Error
                });
            }

            return BadRequest(new
            {
                message = result.Error
            });
        }

        return Ok(result.Product);
    }
}