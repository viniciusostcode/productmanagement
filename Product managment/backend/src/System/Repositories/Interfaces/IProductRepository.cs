using backend.Models;
namespace backend.Repositories.Interfaces

{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetAll();
        Task<ProductModel> FindById(int id);
        Task<List<ProductModel>> FindByUser(string user);
        Task<List<ProductModel>> AddProductList(List<ProductModel> list, string user);
        Task<ProductModel> AddProduct(string user , ProductModel product);
        Task<ProductModel> UpdateProduct(int id, string user, ProductModel productModel);
        Task<bool> DeleteProduct(int id, string user);
    }
}
