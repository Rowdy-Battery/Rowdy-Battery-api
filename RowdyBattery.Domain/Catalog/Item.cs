using System;
using System.Collections.Generic;

namespace RowdyBattery.Domain.Catalog
{
    public class Item
    {
        // Use an integer Id because the API and data seeding expect an int identifier
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Ratings navigation property
        public List<Rating> Ratings { get; set; } = new();

        // Parameterless ctor needed by EF
        public Item() { }

        // Convenience ctor used by seeding and controllers
        public Item(int id, string name, decimal price)
        {
            Id = id;
            Name = name ?? string.Empty;
            Price = price;
        }

        public void AddRating(Rating rating)
        {
            if (rating == null) throw new ArgumentNullException(nameof(rating));
            Ratings.Add(rating);
        }
    }
}
