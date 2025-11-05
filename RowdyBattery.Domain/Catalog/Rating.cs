namespace RowdyBattery.Domain.Catalog;

public class Rating
{
    public int Id { get; private set; }            // needed later for EF
    public int Stars { get; private set; }         // 1..5
    public string UserName { get; private set; }
    public string? Review { get; private set; }

    public Rating(int stars, string userName, string? review)
    {
        if (stars < 1 || stars > 5) throw new ArgumentException("Stars must be 1..5");
        if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("Username required");

        Stars = stars;
        UserName = userName;
        Review = review;
    }
}
