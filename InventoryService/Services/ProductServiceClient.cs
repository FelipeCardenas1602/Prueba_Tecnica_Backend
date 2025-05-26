using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace InventoryService.Services;

public class ProductServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ProductServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> ProductExistsAsync(int productId)
    {
        var baseUrl = _configuration["ProductService:BaseUrl"];
        var apiKey = _configuration["ProductService:ApiKey"];

        var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/products/{productId}");
        request.Headers.Add("X-Api-Key", apiKey);

        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode(); // Lanza excepción si no es 200-299
        return true;
    }
}



