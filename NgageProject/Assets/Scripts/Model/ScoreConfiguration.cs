using SimpleSQL;

public class ScoreConfiguration 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int PercentScore { get; set; }
    public int TotalScore { get; set; }
    public int levelId { get; set; }
    public int UnlockScore { get; set; }


}
