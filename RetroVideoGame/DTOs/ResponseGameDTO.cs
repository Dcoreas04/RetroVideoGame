
public class ResponseGameDTO
{
    public string Title { get; set; }
    public string Publisher { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string Console { get; set; }
    public Enum Condition { get; set; }
}