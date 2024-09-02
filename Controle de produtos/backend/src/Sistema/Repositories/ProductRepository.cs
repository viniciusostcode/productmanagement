using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sistema.Data;
using Sistema.Models;
using Sistema.Models.Enums;
using Sistema.Repositories.Interfaces;

namespace Sistema.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductSystemDbContext _dbContextProduct;

        private readonly UserManager<ApplicationUser> _userManager;
        public ProductRepository(ProductSystemDbContext dbContextTransaction, UserManager<ApplicationUser> userManager)
        {
            _dbContextProduct = dbContextTransaction;
            _userManager = userManager;
        }

        public async Task<List<ProductModel>> AddProductList(List<ProductModel> list, string idUser)
        {
            List<ProductModel> ProductList = new List<ProductModel>();

            if (Equals(list.Count, 0)) throw new Exception("No data.");

            foreach (ProductModel productModel in ProductList)
            {
                ProductModel product = new ProductModel();

                product.Price = productModel.Price;
                product.Quantity = productModel.Quantity;
                product.Date = DateTime.Now;
                product.Product = productModel.Product;

                ApplicationUser user = await _userManager.FindByIdAsync(idUser);

                product.User = user;

                if (!Enum.TryParse(productModel.Situation, out SituationEnum situation)) throw new Exception("A situação é inválida.");

                product.Situation = situation.ToString();

                ProductList.Add(product);
            }

            await _dbContextProduct.Products.AddRangeAsync(ProductList);
            await _dbContextProduct.SaveChangesAsync();

            return ProductList;
        }
        public async Task<List<ProductModel>> GetAll()
        {
            List<ProductModel>? result = await _dbContextProduct.Products.Include(x=> x.User).ToListAsync();
            return result;
        }

        public async Task<ProductModel> FindById(int id)
        {

            ProductModel? result = await _dbContextProduct.Products.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
        public async Task<ProductModel> AddProduct(ProductModel productModel, string userName)
        {
            try
            {
                ProductModel product = new ProductModel();

                if (Equals(productModel, null)) throw new Exception("No data.");

                product.Price = productModel.Price;
                product.Quantity = productModel.Quantity;
                product.Date = DateTime.Now;
                product.Product = productModel.Product;

                ApplicationUser user = await _userManager.FindByNameAsync(userName);

                product.IdUser = user.Id;

                product.User = user;

                if (!Enum.TryParse(productModel.Situation, out SituationEnum situation)) throw new Exception("A situação é inválida.");

                product.Situation = situation.ToString();

                await _dbContextProduct.Products.AddAsync(product);

                await _dbContextProduct.SaveChangesAsync();

                return productModel;

            }
            catch (Exception ex)
            {
                throw new Exception("An error ocurred while saving the entities " + ex.Message);
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            ProductModel produto = await FindById(id);

            if (produto.Equals(null)) throw new Exception($"Product not found by id: {id}");

            _dbContextProduct.Products.Remove(produto);

            await _dbContextProduct.SaveChangesAsync();

            return true;
        }


        public async Task<ProductModel> UpdateProduct(ProductModel productModel, int id)
        {
            try
            {
                ProductModel product = await FindById(id);

                if (product.Equals(null)) throw new Exception($"Product not found by id: {id}");

                product.Price = productModel.Price;
                product.Quantity = productModel.Quantity;
                product.Date = productModel.Date;
                product.Product = productModel.Product;
                product.Situation = productModel.Situation;

                await _dbContextProduct.SaveChangesAsync();

                return productModel;

            }
            catch (Exception ex)
            {
                throw new Exception("An error ocurred while updating the entities: " + ex.Message);
            }
        }

        public async Task<List<ProductModel>> FindByUser(string user)
        {
            List<ProductModel>? result = await _dbContextProduct.Products.Include(x => x.User).Where(x => x.User.UserName == user).ToListAsync();

            return result;
        }
    }
}
