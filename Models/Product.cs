namespace FakeStoreApiTest.Models
{
    public class Product
    {
        public int Id;
        public required string Title;
        public required decimal Price;
        public string? Description;
        public string? Category;
        public string? Image;
        public Rating? Rating;
    }

    public class Rating
    {
        public double Rate;
        public int Count;
    }
}
