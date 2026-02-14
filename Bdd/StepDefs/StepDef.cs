using FakeStoreApiTest.Helper;
using FakeStoreApiTest.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Newtonsoft.Json;
using Reqnroll;
using RestSharp;
//[assembly: CollectionBehavior(DisableTestParallelization = false, MaxParallelThreads = 4)]

namespace FakeStoreApiTest.Bdd.StepDefs
{
    [Binding]
    public sealed class StepDef
    {
		private readonly ApiClientHelper _apiClient;
		public RestResponse? productsResponse;
		public List<Product>? products;
		public Product? cheapestProduct;
		public RestResponse? cartResponse;
		public Cart? productAddedToCart;
		public decimal price;

		public StepDef()
		{
			_apiClient = new ApiClientHelper();
		}

		[Given("I retrieve all products in the {string} category")]
		public async Task GivenIRetrieveAllProductsInTheCategory(string category)
		{
			productsResponse = await _apiClient.GetAsync($"/products/category/{category}");
			productsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
			products = JsonConvert.DeserializeObject<List<Product>>(productsResponse.Content ?? string.Empty);
		}

		[When("I select the product with the lowest price in that category")]
		public async Task WhenISelectTheProductWithTheCheapestPriceInTheCategory()
		{
			cheapestProduct = products!.OrderBy(p => p.Price).First();
			price = cheapestProduct.Price;
		}

		[When("I add {int} quantity of that product to user id {int} cart")]
		public async Task WhenIAddQuantityOfThatProductToUserCart(int qty, int userId)
		{
			cartResponse = await _apiClient.PostAsync($"/carts", RequestHepler.CartBody(userId, cheapestProduct!, qty));
			cartResponse.StatusCode.Should().Be(HttpStatusCode.Created);
			productAddedToCart = JsonConvert.DeserializeObject<Cart>(cartResponse.Content ?? string.Empty);
		}

		[Then("the cart should display the selected product for user id {int}")]
		public async Task ThenTheCartShouldDisplayTheSelectedProductForUserId(int userId)
		{
			productAddedToCart.Should().NotBeNull();
			productAddedToCart.UserId.Should().Be(userId);
			productAddedToCart.Products[0].ProductId.Should().Be(cheapestProduct!.Id);
		}

		[Then("the product price should be correct")]
		public async Task ThenTheProductPriceShouldBeCorrect()
		{
			cheapestProduct!.Price.Should().Be(price);
		}

		[Then("the quantity should be {int}")]
		public async Task ThenTheQuantityShouldBe(int qty)
		{
			productAddedToCart!.Products[0].Quantity.Should().Be(qty);
		}
	}
}