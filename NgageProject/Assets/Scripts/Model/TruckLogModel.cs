using System;

public class TruckLogModel
{
    public int id_log { get; set; }
    public int id_user { get; set; }
    public string truck_name { get; set; }
    public string truck_selected { get; set; }
    public int is_correct { get; set; }
    public string correct_truck { get; set; }
    public int score { get; set; }
    public int attempt_no { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public int sequence { get; set; }
}
