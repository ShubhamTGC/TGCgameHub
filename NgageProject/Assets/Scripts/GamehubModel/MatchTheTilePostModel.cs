﻿using System;

public class MatchTheTilePostModel 
{
    public int id_waste { get; set; }
    public string waste { get; set; }
    public int id_waste_type { get; set; }
    public string type { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public int attempt_no { get; set; }
    public int id_user { get; set; }
    public int score { get; set; }
    public int is_correct { get; set; }
    public int Id_Game { get; set; }
}
