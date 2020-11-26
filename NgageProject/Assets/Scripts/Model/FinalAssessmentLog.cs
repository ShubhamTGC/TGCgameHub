using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinalAssessmentLog : MonoBehaviour
{
    public int id_brief_question { get; set; }
    public int id_organization { get; set; }
    public string brief_question { get; set; }
    public object id_brief_category { get; set; }
    public object id_brief_sub_category { get; set; }
    public object question_image { get; set; }
    public int question_type { get; set; }
    public object question_complexity { get; set; }
    public object expiry_date { get; set; }
    public object complexity_label { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public object question_theme_type { get; set; }
    public object question_choice_type { get; set; }
    public int qtnnum { get; set; }
    public List<Answer> answer { get; set; }
    public int Point { get; set; }
    public int Negative_Point { get; set; }
    public int id_level { get; set; }
}


public class Answer
{
    public int id_brief_answer { get; set; }
    public int id_organization { get; set; }
    public int id_brief_question { get; set; }
    public string brief_answer { get; set; }
    public int is_correct_answer { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public object choice_image { get; set; }
    public object choice_type { get; set; }
}






