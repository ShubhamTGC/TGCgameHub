using SimpleSQL;
public class GameSetting 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int Music { get; set; }
    public int Sound { get; set; }
    public int Vibration { get; set; }
}
