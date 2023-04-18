using AutoMapper;
using ECommerceApp.Application.DTO;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Application.IProductRepository;
using ECommerceApp.Application.Specifications;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Persistance.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IMapper _mapper;


        public ProductController(IGenericRepository<Product> productRepository,
            IGenericRepository<ProductBrand> productBrandRepository,
            IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper)
        {

           _productBrandRepository = productBrandRepository;
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepo;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            
            var products = await _productRepository.ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductDto>>(products));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productRepository.GetEntityWithSpec(spec);

            return _mapper.Map<Product,ProductDto>(product);
   
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepository.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepository.ListAllAsync());
        }

    }
}
