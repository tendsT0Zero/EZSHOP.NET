using EzShop.Api.Models;

namespace EzShop.Api.Repository
{
    public interface IProductRepository
    {
        public Task<List<Product>?> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);

        //Create product and return after creation
        Task<Product?>CreateAsync(Product product);
        //Update product and return after updation
        Task<Product?> UpdateAsync(int id, Product product);
        //Delete product and return boolean value based on deletion
        Task<bool> DeleteAsync(int id);
        //search by keyword in name or category
        Task<List<Product>?> SearchAsync(string? keyword);
    }
}
