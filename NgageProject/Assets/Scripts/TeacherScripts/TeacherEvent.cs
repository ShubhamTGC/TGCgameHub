using SimpleSQL;

public class TeacherEvent 
{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public int Date { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string Event { get; set; }
    public int IdEvent { get; set; }

}
