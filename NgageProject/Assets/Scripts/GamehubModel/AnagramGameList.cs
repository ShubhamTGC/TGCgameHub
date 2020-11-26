using SimpleSQL;

public class AnagramGameList 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int IdWord { get; set; }
    public int GameId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public string ImageUrl { get; set; }
    public int CorrectPoint { get; set; }
    public int WrongPoint { get; set; }
    public string BackgroundImage { get; set; }

}
