using SimpleSQL;

public class MonsterSpeed 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int GameId { get; set; }
    public int SpeedId { get; set; }
    public int Score { get; set; }
    public float SpeedValue { get; set; }
    
}
