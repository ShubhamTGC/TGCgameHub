using SimpleSQL;

public class TruckCenterDetails 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int TruckId { get; set; }
    public string CenterName { get; set; }
    public int CorrectPoint { get; set; }
    public int WrongPoint { get; set; }
    public int ScoreId { get; set; }


}
