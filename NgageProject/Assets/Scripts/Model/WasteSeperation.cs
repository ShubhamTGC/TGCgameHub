using SimpleSQL;
public class WasteSeperation 
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int RoomId { get; set; }
    public int PCscore { get; set; }
    public int Cscore { get; set; }
}
