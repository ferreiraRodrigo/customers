using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Customers.Infra.Adapters.Product.Interfaces
{
    public class ProductAdapter : IProductAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly string _resource;

        public ProductAdapter(HttpClient httpClientFactory, IOptions<ProductAdataperConfigurations> options)
        {
            _httpClient = httpClientFactory;
            _httpClient.BaseAddress = new Uri(options.Value.BaseAddress);

            _resource = options.Value.Resource;
        }

        public async Task<ProductDTO> GetProduct(Guid productId)
        {
            var uri = $"{_resource}/{productId}/";
            
            var response = await _httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<ProductDTO>(content, options);
        }
    }
}
 