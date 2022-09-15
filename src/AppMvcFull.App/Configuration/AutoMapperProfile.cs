using AppMvcFull.App.ViewModels;
using AppMvcFull.Business.Models;
using AutoMapper;

namespace AppMvcFull.App.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Supplier, SupplierViewModel>().ReverseMap();

            CreateMap<Address, AddressViewModel>().ReverseMap();

            CreateMap<Product, ProductViewModel>();

            CreateMap<ProductViewModel, Product>()
                  .ForMember(dist => dist.Supplier, src => src.Ignore());


        }
    }
}
