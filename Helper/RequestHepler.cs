using FakeStoreApiTest.Models;

namespace FakeStoreApiTest.Helper
{
    public static class RequestHepler
    {
		public static object ProductBody(string title, decimal price, string description, string image, string category)
		{
			return new
			{
				Title = title,
				Price = price,
				Description = description,
				Image = image,
				Category = category
			};
		}

		public static object CartBody(int userIdNo, Product product, int qty)
		{
			return new
			{
				userId = userIdNo,
				date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
				products = new[]
				{
                    new
					{
						productId = product.Id,
						quantity = qty
					}
				}
			};
		}
	}
}
