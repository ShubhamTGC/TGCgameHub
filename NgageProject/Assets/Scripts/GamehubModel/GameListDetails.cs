using SimpleSQL;

public class GameListDetails 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int GameId { get; set; }
    public int Oid { get; set; }
    public string GameName { get; set; }
    public string GameType { get; set; }
    public int UpdateFlag { get; set; }
    public int CompletePer { get; set; }
    public string RoundImageUrl { get; set; }
    public string RectImageUrl { get; set; }


}
