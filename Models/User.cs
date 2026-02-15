
namespace FakeStoreApiTest.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public Name Name { get; set; } = new Name();
	}

	public class Name
	{
		public string Firstname { get; set; } = string.Empty;
		public string Lastname { get; set; } = string.Empty;
	}
}