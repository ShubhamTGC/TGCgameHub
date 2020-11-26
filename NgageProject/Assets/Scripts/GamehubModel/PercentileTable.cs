using SimpleSQL;

public class PercentileTable 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int PercentileId { get; set; }
    public int Percentile { get; set; }
  
}
