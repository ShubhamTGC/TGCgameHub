using SimpleSQL;

public class LevelPercentageTable 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int LevelId { get; set; }
    public int LevelPercentage { get; set; }

}
