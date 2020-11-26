using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



public class Dashboard
{
    public List<DashboardItem> Items { get; set; }
}


public class UserLog
{
    public int id_log { get; set; }
    public string item_collected { get; set; }
    public int score { get; set; }
    public int is_right { get; set; }
    public string correct_option { get; set; }
    public int id_content { get; set; }
    public int id_level { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public int id_user { get; set; }
    public string dustbin { get; set; }
    public int id_room { get; set; }
    public int attempt_no { get; set; }



}

public class ContentList
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
    public int totalscore { get; set; }
    public List<UserLog> UserLog { get; set; }

}

public class DashboardItem
{
    public int UID { get; set; }
    public int OID { get; set; }
    public int id_game { get; set; }
    public int id_level { get; set; }
    public List<ContentList> ContentList { get; set; }

}




