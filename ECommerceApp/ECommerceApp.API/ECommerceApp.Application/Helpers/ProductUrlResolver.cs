using AutoMapper;
using AutoMapper.Execution;
using ECommerceApp.Application.DTO;
using ECommerceApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Application.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _config;

        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
