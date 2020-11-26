using SimpleSQL;

public class BonusTable 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int Time0to30 { get; set; }
    public int BonusPoint1 { get; set; }
    public int Time30to45 { get; set; }
    public int BonusPoint2 { get; set; }

}
