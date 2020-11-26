using System;

public class TruckSeqModel 
{
    public int id_truck { get; set; }
    public string truck_name { get; set; }
    public int sequence { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public int correct_priority_point { get; set; }
    public int wrong_point { get; set; }
}
