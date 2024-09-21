using frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

[Authorize]
public class ProductsController : Controller
{
    public IActionResult Index()
    {
        var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Unknown";
        ViewData["UserName"] = userName;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsData()
    {
        var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Unknown";

        var token = Request.Cookies["AuthToken"];

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"https://localhost:7215/products/find/{userName}");

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

    [HttpPut]
    public async Task<IActionResult> UpdateProductsData(decimal id, string product)
    {
        var token = Request.Cookies["AuthToken"];

        using (var client = new HttpClient())
        {
            var jsonString = JsonSerializer.Serialize(product);
            var productUpdated = new StringContent(jsonString, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PutAsync($"https://localhost:7215/products/{id}", productUpdated);

            if (response.IsSuccessStatusCode)
            {
                var products = JsonSerializer.Deserialize<List<ProductModel>>(await response.Content.ReadAsStringAsync());
                return Json(products);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(decimal id)
    {
        var token = Request.Cookies["AuthToken"];

        using (var client = new HttpClient())
        {

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync($"https://localhost:7215/products/{id}");

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