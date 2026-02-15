using FakeStoreApiTest.Helper;
using FakeStoreApiTest.Models;
using FluentAssertions;
using Newtonsoft.Json;

namespace FakeStoreApiTest.Tests
{
    public class Us1AddProductToCartTests
    {
        private readonly ApiClientHelper _apiClient;

        public Us1AddProductToCartTests()
        {
            _apiClient = new ApiClientHelper();
        }

        [Theory(DisplayName = "As an online shopper, I want to add the cheapest product in a selected category to my cart")]
        [InlineData("electronics")]
        [InlineData("men's clothing")]
        [InlineData("jewelery")]
        [InlineData("women's clothing")]
        public async Task Should_Retrieve_Products_By_Category_And_Add_Cheapest_To_Cart(string category)
        {
			int cheapestProductId;
			int userId = 1;
			int quantity = 1;

			//Get products by category electronics
			var productsResponse = await _apiClient.GetAsync($"/products/category/{category}");
            productsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

			var products = JsonConvert.DeserializeObject<List<Product>>(productsResponse.Content!);
			products.Should().NotBeEmpty();

            //Get cheapest product by ordering it in ascending order lowest to highest
            var cheapestProduct = products
                .OrderBy(p => p.Price)
                .First();

            cheapestProduct.Should().NotBeNull();
			cheapestProductId = cheapestProduct.Id;

			//Create cart payload
			var cartPayload = RequestHepler.CartBody(userId, cheapestProduct, quantity);

            //Add to cart
            var cartResponse = await _apiClient.PostAsync("/carts", cartPayload);
            cartResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            //Test 1: Check that newly added item is displayed
            var cartResult = JsonConvert.DeserializeObject<Cart>(cartResponse.Content!);
            cartResult.Should().NotBeNull();
            cartResult.UserId.Should().Be(userId);
			cartResult.Products.Should().Contain
                (p => p.ProductId == cheapestProductId
			   && p.Quantity == quantity);

			////Test 2: Check that after adding item that item is displayed in cart via productId we just added
            //var cartUserResponse = await _apiClient.GetAsync($"/carts/user/{userId}");
            //         cartUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            //         var cartUserResult = JsonConvert.DeserializeObject<List<Cart>>(cartUserResponse.Content!);
            //		  cartUserResult.Should()
            //             .Contain(cart => cart.Products
            //             .Any(p => p.ProductId == cheapestProductId && p.Quantity == quantity));
        }
    }
}