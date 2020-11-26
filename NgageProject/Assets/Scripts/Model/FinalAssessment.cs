using SimpleSQL;

public class FinalAssessment

{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public int QuesId {get;set;}
    public string Question { get; set; }
    public string Options { get; set; }
    public string OptionsID { get; set; }
    public string CorrectAns { get; set; }
    public int Levelid { get; set; }

}
