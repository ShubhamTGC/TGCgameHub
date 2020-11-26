
using SimpleSQL;

public class ParentChildLog 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public string ParentName { get; set; }
    public int IdChild { get; set; }
}
