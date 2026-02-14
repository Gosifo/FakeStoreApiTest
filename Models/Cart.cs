
namespace FakeStoreApiTest.Models
{
    public class Cart
    {
		public required int Id;
		public required int UserId;
		public required string Date;
		public required List<CartProduct> Products;
	}

	public class CartProduct
	{
		public required int ProductId;
		public required int Quantity;
	}
}