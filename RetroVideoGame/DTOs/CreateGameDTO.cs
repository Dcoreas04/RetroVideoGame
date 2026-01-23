
public class CreateGameDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Publisher { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string Console { get; set; }
    public Enum Condition { get; set; }
}