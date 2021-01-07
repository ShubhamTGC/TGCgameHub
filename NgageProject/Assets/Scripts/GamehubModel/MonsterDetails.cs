using SimpleSQL;

public class MonsterDetails 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int MonsterId { get; set; }
    public int CatchPoint { get; set; }
    public int GameId { get; set; }
    public string MonsterImgUrl { get; set; }

    
}
