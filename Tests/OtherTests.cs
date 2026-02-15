using FakeStoreApiTest.Helper;
using FakeStoreApiTest.Models;
using FluentAssertions;
using Newtonsoft.Json;

namespace FakeStoreApiTest.Tests
{
    public class OtherTests
	{
        private readonly ApiClientHelper _apiClient;

        public OtherTests()
        {
            _apiClient = new ApiClientHelper();
        }

		[Theory(DisplayName = "Verify that we can delete cart")]
		[InlineData(2)]
		[InlineData(6)]
		public async Task Should_Delete_Cart(int id)

        {
			//Delete cart by id
			var response = await _apiClient.DeleteAsync($"/carts/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

			//Checks delete cart
			var cartDeletedResponse = JsonConvert.DeserializeObject<Cart>(response.Content!);
			cartDeletedResponse.Should().NotBeNull();
            cartDeletedResponse.Id.Should().Be(id);
		}


		[Fact(DisplayName = "Test that user can login successfully with valid details")]
		public async Task Should_Login_Successfully()
		{
			string userName;
			string pwd;

			var userResponse = await _apiClient.GetAsync("/users");
			userResponse.StatusCode.Should().Be(HttpStatusCode.OK);

			var userData = JsonConvert.DeserializeObject<List<User>>(userResponse.Content!);
			userData.Should().NotBeNull();
			var getFirstUserData = userData.FirstOrDefault();
			userName = getFirstUserData!.Username;
			pwd = getFirstUserData!.Password;

			var authPayload = new
			{
				username = userName,
				password = pwd
			};

			var authResponse = await _apiClient.PostAsync("/auth/login", authPayload);
			authResponse.StatusCode.Should().Be(HttpStatusCode.Created);
			authResponse.Content.Should().NotBeNull();
			authResponse.Content.Should().Contain("token");
		}


		[Fact(DisplayName = "Test that user cannot login successfully with invalid details")]
		public async Task Should_Not_Login_Successfully()
		{
			string userName = "invalidUser";
			string pwd = "hjsfh23£%^";

			var authPayload = new
			{
				username = userName,
				password = pwd
			};

			var authResponse = await _apiClient.PostAsync("/auth/login", authPayload);
			authResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
			authResponse.Content.Should().NotBeNull();
			authResponse.Content.Should().Be("username or password is incorrect");
		}
	}
}