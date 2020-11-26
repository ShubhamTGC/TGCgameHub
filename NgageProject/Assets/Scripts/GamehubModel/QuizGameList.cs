using SimpleSQL;

public class QuizGameList 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int GameId { get; set; }
    public int CorrectPoint { get; set; }
    public int WrongPoint { get; set; }
    public string Question { get; set; }
    public string CorrectOption { get; set; }
    public string Option1 { get; set; }
    public string Option2 { get; set; }
    public string Option3 { get; set; }
    public string Option4 { get; set; }
    public int QuizId { get; set; }

}
