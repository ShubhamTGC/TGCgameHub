
public class QuizGamePostModel 
{
    public int id_question { get; set; }
    public int score { get; set; }
    public int is_right { get; set; }
    public string user_Ans { get; set; }
    public int id_user { get; set; }
    public int Id_Game { get; set; }
    public int attempt_no { get; set; }
}
