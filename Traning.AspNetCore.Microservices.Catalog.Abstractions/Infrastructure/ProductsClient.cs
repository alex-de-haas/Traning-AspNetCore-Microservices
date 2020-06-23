using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Catalog.Abstractions.Infrastructure
{
    public class ProductsClient : IProductsClient
    {
        private const string URL = "products";

        private readonly HttpClient _httpClient;

        public ProductsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductViewDto[]> GetProductsAsync(Guid[] productIds = default, CancellationToken cancellationToken = default)
        {
            var uri = URL;
            var query = new NameValueCollection();
            if (productIds != null)
            {
                foreach (var productId in productIds)
                {
                    query.Add("productIds", productId.ToString());
                }
            }
            if (query.Count > 0)
            {
                uri += ToQueryString(query);
            }
            using (var response = await _httpClient.GetAsync(uri, cancellationToken))
            {
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<ProductViewDto[]>(responseString);
                return result;
            }
        }

        public async Task<ProductViewDto> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            using (var response = await _httpClient.GetAsync($"{URL}/{productId}", cancellationToken))
            {
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<ProductViewDto>(responseString);
                return result;
            }
        }

        public async Task<Guid> CreateProductAsync(ProductCreateDto product, CancellationToken cancellationToken = default)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            using (var request = new HttpRequestMessage(HttpMethod.Post, URL))
            {
                var data = JsonConvert.SerializeObject(product);
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var response = await _httpClient.SendAsync(request, cancellationToken))
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Guid>(content);
                }
            }
        }

        public async Task UpdateProductAsync(Guid productId, ProductUpdateDto product, CancellationToken cancellationToken = default)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            using (var request = new HttpRequestMessage(HttpMethod.Put, $"{URL}/{productId}"))
            {
                var data = JsonConvert.SerializeObject(product);
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var response = await _httpClient.SendAsync(request, cancellationToken))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        public async Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, $"{URL}/{productId}"))
            using (var response = await _httpClient.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (
                from key in nvc.AllKeys
                from value in nvc.GetValues(key)
                select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();
            return "?" + string.Join("&", array);
        }
    }
}
