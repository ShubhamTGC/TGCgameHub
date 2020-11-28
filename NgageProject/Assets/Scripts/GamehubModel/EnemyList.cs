using SimpleSQL;

public class EnemyList
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }
    public int EnemyId { get; set; }
    public int GameId { get; set; }
    public int RoomId { get; set; }
    public string EnemyName { get; set; }
}
