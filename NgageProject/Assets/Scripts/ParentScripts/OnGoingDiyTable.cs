using SimpleSQL;

public class OnGoingDiyTable 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int IdLog { get; set; }
    public int UserId { get; set; }
    public int OID { get; set; }
    public int IdGameContent { get; set; }
    public int IdLevel { get; set; }
    public string PhotoUrl { get; set; }
    public string Detail { get; set; }
    public string DiyDate { get; set; }
}
