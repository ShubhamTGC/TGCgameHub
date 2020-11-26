using SimpleSQL;

public class MatchTheTileLocation 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int LocationTypeId { get; set; }
    public string Type { get; set; }
    public string ImageUrl { get; set; }
    public int GameId { get; set; }

   
}
