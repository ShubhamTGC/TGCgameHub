using System;

public class TruckGetLogModel 
{
    public int id_log { get; set; }
    public int id_truck { get; set; }
    public int id_user { get; set; }
    public int dustbin_collected { get; set; }
    public int truck_score { get; set; }
    public string reached_center { get; set; }
    public int center_score { get; set; }
    public int is_correct_reached { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public int attempt_no { get; set; }
    public string truck_name { get; set; }
}
