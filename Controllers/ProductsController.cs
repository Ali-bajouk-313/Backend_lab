using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.Services;
using AutoMapper;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _environment;

    public ProductsController(
        IMapper mapper,
        IProductService productService,
        IWebHostEnvironment environment)
    {
        _productService = productService;
        _environment = environment;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool onlyAvailable = false)
    {
        var products =
            await _productService.GetAllAsync(onlyAvailable);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        [FromRoute] int id)
    {
        var product =
            await _productService.GetByIdAsync(id);

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
    public async Task<IActionResult> Search(
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

        var products =
            await _productService.SearchAsync(name, supplier);

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request)
    {
        try
        {
            var product =
                await _productService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                product);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("{id:int}/quantity")]
    public async Task<IActionResult> UpdateQuantity(
        [FromRoute] int id,
        [FromBody] UpdateProductQuantityRequest request)
    {
        var result =
            await _productService.UpdateQuantityAsync(
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

    [HttpPost("{id:int}/price")]
    public async Task<IActionResult> UpdatePrice(
        [FromRoute] int id,
        [FromBody] UpdateProductPriceRequest request)
    {
        var result =
            await _productService.UpdatePriceAsync(
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

    [HttpPost("{id:int}/image")]
    public async Task<IActionResult> UploadImage(
        [FromRoute] int id,
        [FromForm] IFormFile file)
    {
        var product =
            await _productService.GetByIdAsync(id);

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
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();

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

        await using FileStream stream =
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int id)
    {
        var result =
            await _productService.ArchiveAsync(id);

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

    [HttpGet("server-time")]
    public IActionResult GetServerTime(
        [FromHeader(Name = "Accept-Language")]
        string? language)
    {
        string cultureName =
            language?.ToLowerInvariant() switch
            {
                "fr-fr" => "fr-FR",
                "ar-lb" => "ar-LB",
                _ => "en-US"
            };

        CultureInfo culture =
            CultureInfo.GetCultureInfo(cultureName);

        string formattedDate =
            DateTime.Now.ToString("F", culture);

        return Ok(new
        {
            language = cultureName,
            serverTime = formattedDate
        });
    }

    [HttpPost("{id:int}/assign-supplier/{supplierId:int}")]
    public async Task<IActionResult> AssignSupplier(
        [FromRoute] int id,
        [FromRoute] int supplierId)
    {
        var result =
            await _productService.AssignSupplierAsync(
                id,
                supplierId);

        if (!result.Success)
        {
            if (result.Error == "Product not found." ||
                result.Error == "Supplier not found.")
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