
public class CreateGameDTO
{
    // AI was used to help me make and understand this class so i can use it as a example of how to make the others
    public int Id { get; set; }
    public string Title { get; set; }
    public string Publisher { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string Console { get; set; }
    public Conditions Condition { get; set; }
}