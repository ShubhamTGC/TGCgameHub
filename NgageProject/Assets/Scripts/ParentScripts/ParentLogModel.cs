
using System.Collections.Generic;
using System;

public class ParentLogModel 
{
    public string ParentName { get; set; }
    public List<ChildDetails> ChildList { get; set; }
}


public class ChildDetails
{

    public string Name { get; set; }
    public int id_user { get; set; }
    public string Grade { get; set; }
    public string School { get; set; }
    public int OverallScore { get; set; }
    public int Zones { get; set; }
    public int BonusScore { get; set; }
    public List<DiyLog> OngingActivity { get; set; }
    public List<DiyLog> UpcomingActivity { get; set; }
    public string BaseUrl { get; set; }



}

public class DiyLog
{
    public int id_log { get; set; }
    public int id_org { get; set; }
    public int id_user { get; set; }
    public int id_game_content { get; set; }
    public int id_level { get; set; }
    public string photo_filename { get; set; }
    public string detail_info { get; set; }
    public DateTime updated_time { get; set; }
    public string status { get; set; }
    public DateTime diy_date_time { get; set; }
}
