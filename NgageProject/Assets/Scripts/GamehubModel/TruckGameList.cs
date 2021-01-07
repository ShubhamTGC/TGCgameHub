using SimpleSQL;

public class TruckGameList 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int GameId { get; set; }
    public string GameName { get; set; }
    public string TruckName { get; set; }
    public int DustbinId { get; set; }
    public string DustbinName { get; set; }
    public int CorrectPoint { get; set; }
    public int WrongPoint { get; set; }
    public int ScoreId { get; set; }
    public int TruckId { get; set; }
    public string TruckImgUrl { get; set; }
    public string CapsuleImgUrl { get; set; }
    public string CenterImgUrl { get; set; }

}
