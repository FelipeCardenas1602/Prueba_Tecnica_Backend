using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("check-product/{id}")]
        public async Task<IActionResult> CheckProduct(int id)
        {
            var client = _httpClientFactory.CreateClient("ProductService");

            var response = await client.GetAsync($"/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Ok("✔ Producto encontrado en ProductService");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized("🔒 Acceso no autorizado al ProductService");
            }
            else
            {
                return StatusCode((int)response.StatusCode, "❌ Otro error al llamar al ProductService");
            }
        }
    }
}
