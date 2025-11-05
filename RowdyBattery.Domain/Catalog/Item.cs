namespace RowdyBattery.Domain.Catalog;

public class Item
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    // Ratings will be used later (EF)
    public List<Rating> Ratings { get; private set; } = new();

    public Item(int id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        if (price < 0) throw new ArgumentException("Price must be >= 0");

        Id = id;
        Name = name;
        Price = price;
    }

    public void AddRating(Rating rating) => Ratings.Add(rating);
}
