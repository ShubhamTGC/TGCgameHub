using SimpleSQL;

public class StageClearness 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int LevelId { get; set; }
    public int IsClear { get; set; }
}
