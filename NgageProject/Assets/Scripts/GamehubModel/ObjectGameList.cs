using SimpleSQL;
public class ObjectGameList 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int GameId { get; set; }
    public int RoomId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int CorrectPoint { get; set; }
    public int WrongPoint { get; set; }
    public int CompletionScore { get; set; }
}
