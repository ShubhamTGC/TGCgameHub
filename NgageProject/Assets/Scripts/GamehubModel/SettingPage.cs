using SimpleSQL;

public class SettingPage 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int Music { get; set; }
    public int Sound { get; set; }
    public int Vibration { get; set; }
    

}
