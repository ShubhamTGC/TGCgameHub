using SimpleSQL;

public class MatchTheTileFlag 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int IdFlag { get; set; }
    public string Flag { get; set; }
    public int LocationMatchId { get; set; }
    public int CorrectPoint { get; set; }
    public int WrongPoint { get; set; }
    public int GameId { get; set; }


}
