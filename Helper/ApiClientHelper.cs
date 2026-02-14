using RestSharp;

namespace FakeStoreApiTest.Helper;

public class ApiClientHelper
{
	private static string BaseUrl => "https://fakestoreapi.com";

	private readonly RestClient _client;

	public ApiClientHelper()
	{
		_client = new RestClient(BaseUrl);
	}

	public async Task<RestResponse> GetAsync(string endpoint)
	{
		var request = new RestRequest(endpoint, Method.Get);
		return await _client.ExecuteAsync(request);
	}

	public async Task<RestResponse> PostAsync(string endpoint, object body)
	{
		var request = new RestRequest(endpoint, Method.Post);
		request.AddJsonBody(body);
		return await _client.ExecuteAsync(request);
	}

	public async Task<RestResponse> DeleteAsync(string endpoint)
	{
		var request = new RestRequest(endpoint, Method.Delete);
		return await _client.ExecuteAsync(request);
	}
}