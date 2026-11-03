using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using API_with_DOCKER.DataBase;
using API_with_DOCKER.models;
using API_with_DOCKER.services;

namespace API_with_DOCKER.Tests
{
    public  class StockConcurrencyTests
    {
       
       

        [Fact]
        public async Task Purchase_WithAvailableStock_ShouldSucceed()
        {
            //arrange 
            using var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8087")
            };
            var product = new
            {
                name = "product1 for test",
                description = "creating product for interation test",
                price = 50m,
                stockQuantity = 10
            };


            //act 
            var createResponse = await client.PostAsJsonAsync("/api/products", product);
            createResponse.EnsureSuccessStatusCode();

            var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

            Assert.NotNull(createdProduct);

            int productId = createdProduct.Id;
            using var response = await client.PostAsync($"/api/inventory/{productId}/purchase", null);


            //assert
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Purchase_100ConcurrentRequests_Only10ShouldSucceed()
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8087")
            };
            var product = new
            {
                name = "product2 for test",
                description = "creating product for interation test",
                price = 50m,
                stockQuantity = 10
            };


            //act 
            var createResponse = await client.PostAsJsonAsync("/api/products", product);
            createResponse.EnsureSuccessStatusCode();

            var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

            Assert.NotNull(createdProduct);

            int productId = createdProduct.Id;
            var tasks = Enumerable.Range(0, 100).Select(_ => client.PostAsync($"/api/inventory/{productId}/purchase",null)).ToArray();

            var responses = await Task.WhenAll(tasks);


            //assert

            int successCount = responses.Count(r => r.StatusCode == HttpStatusCode.OK);

            int conflictCount = responses.Count(r => r.StatusCode == HttpStatusCode.Conflict);

            Assert.Equal(100, responses.Length);
            Assert.Equal(10, successCount);
            Assert.Equal(90, conflictCount);
            var finalProduct = await client.GetFromJsonAsync<Product>($"/api/products/{productId}");


            Assert.Equal(0, finalProduct.StockQuantity);
        }

    }
}
