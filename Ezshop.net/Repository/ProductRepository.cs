using EzShop.Api.Data;
using EzShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EzShop.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;
        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Product>?> GetAllProductsAsync()
        {
            var  products = await context.Products.Include(u=>u.Image).ToListAsync();
            if(products is null )
            {
                return null;
            }
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product =await context.Products.Include(u=>u.Image).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
            {
                return null;
            }
            return  product;
        }

        public async Task<Product?> CreateAsync(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existngProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existngProduct != null)
            {
                context.Products.Remove(existngProduct);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Product>?> SearchAsync(string? keyword)
        {
            if(string.IsNullOrWhiteSpace(keyword))
            {
                return null;
            } 
            keyword= keyword.ToLower().Trim();
            var searchResults = await context.Products.Include(u => u.Image)
                .Where(p => p.Name.Contains(keyword) || p.Category.Contains(keyword))
                .ToListAsync();
            if (searchResults is null || searchResults.Count == 0)
            {
                return null;
            }
            return searchResults;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existingProduct= await context.Products.Include(u => u.Image).FirstOrDefaultAsync(p => p.Id == id);
            if (existingProduct is null)
            {
                return null;
            }
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Category = product.Category;
            existingProduct.Description = product.Description;
            await context.SaveChangesAsync();
            return existingProduct;
        }
    }
}
