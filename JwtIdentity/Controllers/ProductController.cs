using EzShop.Api.DTOs;
using EzShop.Api.Models;
using EzShop.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [Authorize(Roles ="Reader")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productRepository.GetAllProductsAsync();
            if (products is null || products.Count == 0)
            {
                return NotFound("No products found.");
            }
            List<ProductDTO> productDTOs = new();
            foreach(var product in products)
            {
                productDTOs.Add(new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    ImageUrl = product.Image != null? product.Image.FilePath: null,
                    Category = product.Category,
                    Stock = product.Stock,
                    Price = product.Price,
                    Description = product.Description
                });
            }
            return Ok(productDTOs);
        }
        [Authorize(Roles = "Reader")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product is null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.Image.FilePath,
                Category = product.Category,
                Stock = product.Stock,
                Price = product.Price,
                Description = product.Description
            };
            return Ok(productDTO);
        }
        [Authorize(Roles = "Writer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest createProductRequest)
        {
            var product = new Product
            {
                Name = createProductRequest.Name,
                Price = createProductRequest.Price,
                Stock = createProductRequest.Stock,
                Category = createProductRequest.Category,
                Description = createProductRequest.Description,
                ImageId = createProductRequest.ImageId
            };
            var createdProduct = await productRepository.CreateAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }
        [Authorize(Roles = "Reader")]
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] string searchRequest)
        {
            var results = await productRepository.SearchAsync(searchRequest);
            if (results is null || results.Count == 0)
            {
                return NotFound("No products found matching the search criteria.");
            }
            return Ok(results);
        }
        [Authorize(Roles = "Writer")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var isDeleted = await productRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return NoContent();
        }
        [Authorize(Roles = "Writer")]
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateProductRequest updateProductRequest)
        {
            var product = new Product
            {
                Name = updateProductRequest.Name,
                Price = updateProductRequest.Price,
                Stock = updateProductRequest.Stock,
                Category = updateProductRequest.Category,
                Description = updateProductRequest.Description,
                ImageId=updateProductRequest.ImageId,
            };
            var updatedProduct = await productRepository.UpdateAsync(id, product);
            if (updatedProduct is null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(updatedProduct);
        }

    }
}
