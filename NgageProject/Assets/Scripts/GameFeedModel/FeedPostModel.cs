using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedPostModel 
{
    public int id_user { get; set; }
    public int id_log { get; set; }
    public int feed_type { get; set; }
    public string user_name { get; set; }
    public GeneralData general_data { get; set; }
    public DIYLog DIYLog { get; set; }
    public TagLog TagLog { get; set; }
    public List<Comment> Comments { get; set; }
    public int average_rating { get; set; }
    public int bonus_points { get; set; }
    public int like_count { get; set; }
    public object school { get; set; }
    public string Grade { get; set; }
    public int avatar_type { get; set; }
    public int body_type { get; set; }
    public int is_liked { get; set; }
    public string Gender { get; set; }
}


public class DIYLog
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
}

public class GeneralData
{
    public int id_data { get; set; }
    public int id_user { get; set; }
    public string image { get; set; }
    public string description { get; set; }
    public string title { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public string extn { get; set; }
}

public class Comment
{
    public int id_comment { get; set; }
    public int id_log { get; set; }
    public int id_user { get; set; }
    public string comment { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public string FIRSTNAME { get; set; }
    public string LASTNAME { get; set; }
}


public class TagLog
{
    public int id_org { get; set; }
    public int id_user { get; set; }
    public int id_game_content { get; set; }
    public int id_level { get; set; }
    public string photo_filename { get; set; }
    public string id_lati { get; set; }
    public string id_long { get; set; }
    public string detail_info { get; set; }
    public string key_info { get; set; }
    public string status { get; set; }
    public int id_tag_photo { get; set; }
}

