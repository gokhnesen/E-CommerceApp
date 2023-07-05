using AutoMapper;
using ECommerceApp.Application.DTO;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Application.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, o => o.MapFrom(s => s.ProductType.Name));
            //.ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>())

            CreateMap<Domain.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Domain.Entities.OrderAggregate.Address>();

        }
    }
}
