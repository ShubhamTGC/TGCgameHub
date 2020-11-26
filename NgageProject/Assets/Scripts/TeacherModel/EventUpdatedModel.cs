using System;

public class EventUpdatedModel 
{
    public int id_event { get; set; }
    public int id_user { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set;}
    public DateTime event_date { get; set; }
}
