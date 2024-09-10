using backend.Models;
namespace backend.Repositories.Interfaces

{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetAll();
        Task<ProductModel> FindById(int id);
        Task<List<ProductModel>> FindByUser(string user);
        Task<List<ProductModel>> AddProductList(List<ProductModel> list, string idUsuario);
        Task<ProductModel> AddProduct(ProductModel produto, string idUsuario);
        Task<ProductModel> UpdateProduct(ProductModel produtoModel, int id);
        Task<bool> DeleteProduct(int id);
    }
}
