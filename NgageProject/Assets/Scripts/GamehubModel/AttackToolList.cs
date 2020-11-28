using SimpleSQL;

public class AttackToolList 
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }
    public int ToolId { get; set; }
    public int GameId { get; set; }
    public int RoomId { get; set; }
    public string ToolName { get; set; }
}
