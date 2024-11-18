using frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

[Route("Products")]
[Authorize]
public class ProductsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetProductsData")]
    public async Task<IActionResult> GetProductsData()
    {
        var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Unknown";

        var token = Request.Cookies["AuthToken"];

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"http://backend:8080/products/find/{userName}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<ProductModel>>(jsonString);
                return Json(products);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }

    [HttpPut("UpdateProductData/{id}")]
    public async Task<IActionResult> UpdateProductData(decimal id, [FromBody] ProductModel product)
    {
        var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Unknown";

        var token = Request.Cookies["AuthToken"];

        using (var client = new HttpClient())
        {
            var jsonString = JsonSerializer.Serialize(product);
            var productUpdated = new StringContent(jsonString, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PutAsync($"http://backend:8080/products/{userName}/{id}", productUpdated);

            if (response.IsSuccessStatusCode)
            {
                var products = JsonSerializer.Deserialize<ProductModel>(await response.Content.ReadAsStringAsync());
                return Json(products);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }

    [HttpPost("AddProductData")]
    public async Task<IActionResult> AddProductData([FromBody] ProductModel product)
    {
        var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Unknown";

        var token = Request.Cookies["AuthToken"];

        using (var client = new HttpClient())
        {
            var jsonString = JsonSerializer.Serialize(product);
            var AddedProduct = new StringContent(jsonString, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync($"http://backend:8080/products/add/{userName}", AddedProduct);

            if (response.IsSuccessStatusCode)
            {
                var products = JsonSerializer.Deserialize<ProductModel>(await response.Content.ReadAsStringAsync());
                return Json(products);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }

    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(decimal id)
    {
        var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Unknown";

        var token = Request.Cookies["AuthToken"];

        using (var client = new HttpClient())
        {

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync($"http://backend:8080/products/{userName}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}