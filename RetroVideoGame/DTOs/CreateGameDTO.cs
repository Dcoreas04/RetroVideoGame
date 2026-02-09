
public class CreateGameDTO
{
    // AI was used to help me make and understand this class so i can use it as a example of how to make the others
    public string Title { get; set; }
    public string Publisher { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string Platform { get; set; }
    public Conditions Condition { get; set; }

    public int? UserId { get; set; }
}