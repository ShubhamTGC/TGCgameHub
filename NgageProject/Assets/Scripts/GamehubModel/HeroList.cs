using SimpleSQL;

public class HeroList 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int HeroId { get; set; }
    public int GameId { get; set; }
    public int RoomId { get; set; }
    public string HeroName { get; set; }
   
}
