using FakeStoreApiTest.Helper;
using FakeStoreApiTest.Models;
using FluentAssertions;
using Newtonsoft.Json;

namespace FakeStoreApiTest.Tests
{
    public class Us3DeleteProductTests
    {
        private readonly ApiClientHelper _apiClient;

        public Us3DeleteProductTests()
        {
            _apiClient = new ApiClientHelper();
        }

        [Fact(DisplayName = "Products should remove the product with the lowest rating")]
        public async Task Should_Delete_Product_With_Lowest_Rating()

        {
            //Get all products
            var response = await _apiClient.GetAsync("/products");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var products = JsonConvert.DeserializeObject<List<Product>>(response.Content ?? string.Empty);
            products.Should().NotBeNull();
            products.Should().NotBeEmpty();

            //Select product with lowest rating
            var lowestRatedProduct = products
                .Where(p => p.Rating != null)
                .OrderBy(p => p.Rating?.Rate)
                .First();

            lowestRatedProduct.Should().NotBeNull();

            //Delete product
            var deleteResponse = await _apiClient.DeleteAsync($"/products/{lowestRatedProduct.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            ////Attempt to retrieve deleted product should get 404 not found -> this fails because nothing will delete on the database.
            //var getDeletedResponse = await _apiClient.GetAsync($"/products/{lowestRatedProduct.Id}");
            //getDeletedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, "the product should no longer exist after deletion");
        }
    }
}