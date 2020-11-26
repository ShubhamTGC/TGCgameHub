
using System;

public class AssessmentPostLog 
{
    public int id_log { get; set; }
    public int id_user { get; set; }
    public int id_org_game { get; set; }
    public int id_org_game_content { get; set; }
    public int attempt_no { get; set; }
    public int id_org_game_level { get; set; }
    public int id_question { get; set; }
    public int id_answer_selected { get; set; }
    public int is_correct { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
}
