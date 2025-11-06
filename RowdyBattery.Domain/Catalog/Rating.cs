namespace RowdyBattery.Domain.Catalog
{
    public class Rating
    {
        public int Stars { get; set; }
        public string UserName { get; set; }
    public string Review { get; set; }

    public Rating(int stars, string userName, string? review)
        {
            if (stars < 1 || stars > 5)
            {
                throw new System.ArgumentOutOfRangeException(nameof(stars), "Stars must be between 1 and 5.");
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new System.ArgumentException("UserName cannot be null or empty.", nameof(userName));
            }

            this.Stars = stars;
            this.UserName = userName;
            this.Review = review ?? string.Empty;
        }
    }
}
