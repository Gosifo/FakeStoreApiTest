using FakeStoreApiTest.Helper;
using FakeStoreApiTest.Models;
using FluentAssertions;
using Newtonsoft.Json;

namespace FakeStoreApiTest.Tests
{
	public class Us2CreateProductTests
	{
		private readonly ApiClientHelper _apiClient;

		public Us2CreateProductTests()
		{
			_apiClient = new ApiClientHelper();
		}

		// This will fail because data will not really be insert into the database and just return a fake id each time a product is added.
		[Fact(DisplayName = "Create multiple products and verify each has a unique ID")]
		public async Task Should_Create_Multiple_Products_With_Unique_Ids()
		{
            var uniqueSuffix = Guid.NewGuid().ToString().Substring(0, 4);
			var title = $"Test Men's Cotton Jacket {uniqueSuffix}";
			var price = 149.99M;
			var description = "New clothing item A";
			var image = "https://test1Image";
			var category = "men's clothing";

			var productA = RequestHepler.ProductBody(title, price, description, image, category);
			var productB = RequestHepler.ProductBody("Clothing Item B", 182.30m, "New clothing item B", "https://test2Image", "men's clothing");

			// Add the first product and check that a new product is added
			var response1 = await _apiClient.PostAsync("/products", productA);
			response1.StatusCode.Should().Be(HttpStatusCode.Created);
			var createdProduct1 = JsonConvert.DeserializeObject<Product>(response1.Content ?? string.Empty);
			createdProduct1.Should().NotBeNull();
			createdProduct1.Title.Should().Be(title);
			createdProduct1.Price.Should().Be(price);
			createdProduct1.Description.Should().Be(description);
			createdProduct1.Image.Should().Be(image);
			createdProduct1.Category.Should().Be(category);

			var newlyCreatedProdId = createdProduct1.Id;

			// Add the second product and check that a new product is added
			var response2 = await _apiClient.PostAsync("/products", productB);
			response2.StatusCode.Should().Be(HttpStatusCode.Conflict, "Because duplicate product id should not be allowed");
			var createdProduct2 = JsonConvert.DeserializeObject<Product>(response2.Content ?? string.Empty);
			createdProduct2.Should().NotBeNull();
			createdProduct2.Id.Should().NotBe(newlyCreatedProdId);
		}



		// This will fail because data will not really be insert into the database and just return a fake id each time a product is added.
		[Fact(DisplayName = "Products should reject duplicate product name")]
		public async Task Should_Return_Conflict_When_Creating_Product_With_Duplicate_Name()
		{
			//Get existing Product title to be used for test 
			var response = await _apiClient.GetAsync("/products");
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var products = JsonConvert.DeserializeObject<List<Product>>(response.Content ?? string.Empty);
			var chooseProduct = products?.FirstOrDefault();
			var productTitle = chooseProduct!.Title;

			//Product with name/title already exists in the store via above code
			var duplicateProduct = RequestHepler.ProductBody(productTitle, 149, "New clothing item A", "https://test1Image", "men's clothing");

			var response2 = await _apiClient.PostAsync("/products", duplicateProduct);
			response2.StatusCode.Should().Be(HttpStatusCode.Conflict);
		}



		// This will fail
		[Fact(DisplayName = "Products should reject duplicate product names when creating multiple products")]
		public async Task Should_Reject_Duplicate_Product_Names_When_Creating_Multiple_Products()
		{
			var productA = RequestHepler.ProductBody("Clothing Item A {uniqueSuffix}", 149, "New clothing item A", "https://test1Image", "men's clothing");
			var productB = RequestHepler.ProductBody("Clothing Item B", 182, "New clothing item B", "https://test2Image", "men's clothing");
			var duplicateProductB = RequestHepler.ProductBody("Clothing Item B", 196, "New clothing item C", "https://test3Image", "men's clothing");

			// Add product and assert
			var response1 = await _apiClient.PostAsync("/products", productA);
			response1.StatusCode.Should().Be(HttpStatusCode.Created);

			var response2 = await _apiClient.PostAsync("/products", productB);
			response2.StatusCode.Should().Be(HttpStatusCode.Created);

			var response3 = await _apiClient.PostAsync("/products", duplicateProductB);
			response3.StatusCode.Should().Be(HttpStatusCode.Conflict,"Because duplicate product names should not be allowed");
		}



		[Fact(DisplayName = "Create new product and verify it is returned correctly by the API")]
		public async Task Should_Create_Product_And_Return_Correct_Details()
		{
			var uniqueSuffix = Guid.NewGuid().ToString().Substring(0, 4);
			var title = $"Test Men's Cotton Jacket {uniqueSuffix}";
			decimal price = 149.99M;
			var description = "New clothing item A";
			var image = "https://test1Image";
			var category = "men's clothing";

			var productsToCreate = RequestHepler.ProductBody(title, price, description, image, category);

			var response = await _apiClient.PostAsync("/products", productsToCreate);
			response.StatusCode.Should().Be(HttpStatusCode.Created);

			// Test 1: Checks the newly added product is immediately visible.
			var createdProduct = JsonConvert.DeserializeObject<Product>(response.Content ?? string.Empty);
			createdProduct.Should().NotBeNull();
			createdProduct.Id.Should().Be(21);
			createdProduct.Title.Should().Be(title);
			createdProduct.Price.Should().Be(price);
			createdProduct.Description.Should().Be(description);
			createdProduct.Image.Should().Be(image);
			createdProduct.Category.Should().Be(category);

			//// Test 2: checks the product API to see if the new product is there
			//var getProductResponse = await _apiClient.GetAsync("/products");
			//getProductResponse.StatusCode.Should().Be(HttpStatusCode.OK);

			//var getProducts = JsonConvert.DeserializeObject<List<Product>>(getProductResponse.Content ?? string.Empty);
			//getProducts.Should().NotBeNull();
			//getProducts.Should().Contain(p => p.Id == createdProduct.Id);
		}
	}
}