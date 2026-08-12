using AutoMapper;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.ViewModels;

namespace WarehouseManagement.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductViewModel>()
            .ForMember(
                dest => dest.SupplierName,
                opt => opt.MapFrom(
                    src => src.Supplier != null
                        ? src.Supplier.Name
                        : src.SupplierName))
            .ForMember(
                dest => dest.ImagePaths,
                opt => opt.MapFrom(
                    src => src.Images
                        .Select(i => i.FilePath)
                        .ToList()));

        CreateMap<Supplier, SupplierViewModel>();
    }
}