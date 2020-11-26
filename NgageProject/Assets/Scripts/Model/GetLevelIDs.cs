
using System;
using System.Collections.Generic;

public class GetLevelIDs
{
    public int UID { get; set; }
    public int OID { get; set; }
    public int id_game { get; set; }
    public int id_level { get; set; }
    public int is_level_completed { get; set; }
    public List<Content> content { get; set; }
    public object level_badge_log { get; set; }
}




public class Content
{
    public int id_game_content { get; set; }
    public int content_type { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public int id_brief_master { get; set; }
    public int id_level { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public int content_sequence { get; set; }
    
}


