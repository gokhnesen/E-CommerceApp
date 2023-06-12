using AutoMapper;
using ECommerceApp.Application.DTO;
using ECommerceApp.Domain.Entities;
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

            CreateMap<AddressDto, AddressDto>().ReverseMap();

        }
    }
}
