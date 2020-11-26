using SimpleSQL;

public class ChildDetailLog 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public string ChildName { get; set; }
    public int IdChild { get; set; }
    public string Grade { get; set; }
    public string School { get; set; }
    public int OverAllScore { get; set; }
    public int Zones { get; set; }
    public int BonusScore { get; set; }
    public string BaseUrl { get; set; }
}
