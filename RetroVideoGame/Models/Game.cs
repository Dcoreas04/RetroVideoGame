public class Game
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Publisher { get; set; }

    public DateOnly ReleaseDate { get; set; } = new DateOnly();

    public string Platform { get; set; }

    public Conditions Condition { get; set; }
}