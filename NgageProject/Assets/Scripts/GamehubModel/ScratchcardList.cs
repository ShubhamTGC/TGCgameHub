using SimpleSQL;

public class ScratchcardList 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int CardId { get; set; }
    public string CardText { get; set; }
    public int CardValue { get; set; }
    public int OID { get; set; }

}
