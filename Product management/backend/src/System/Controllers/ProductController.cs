using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Route("products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
       {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductModel>>> GetAll()
        {
            try
            {
                List<ProductModel> products = await _productRepository.GetAll();

                if (products != null) return Ok(products);

                return NotFound("Result not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> FindProductById(int id)
        {
            try
            {
                ProductModel product = await _productRepository.FindById(id);

                if (product != null) return Ok(product);

                return NotFound("Result not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        
        [HttpGet("find/{user}")]
        public async Task<ActionResult<List<ProductModel>>> FindProductByUser(string user)
        {
            try
            {
                List<ProductModel> product = await _productRepository.FindByUser(user);

                if (product != null) return Ok(product);

                return NotFound("Result not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("add/{user}")]
        public async Task<ActionResult<ProductModel>> AddProduct([FromBody] ProductModel productModel, string user)
        {

            ProductModel product = await _productRepository.AddProduct(productModel, user);

            return Ok(product);
        }

        [HttpPost("list")]
        public async Task<ActionResult<ProductModel>> AddProductList([FromBody] List<ProductModel> productModel, string idUser)
        {
            List<ProductModel> product = await _productRepository.AddProductList(productModel, idUser);

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> UpdateProduct(int id, [FromBody] ProductModel productModel)
        {
            ProductModel product = await _productRepository.UpdateProduct(productModel, id);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductModel>> DeleteProduct(int id)
        {
            bool deleted = await _productRepository.DeleteProduct(id);

            return Ok(deleted);
        }

    }
}
