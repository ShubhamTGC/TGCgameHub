using System;

public class VideoUrlModel 
{
    public int id_video { get; set; }
    public string video_url { get; set; }
    public int id_level { get; set; }
    public int id_zone { get; set; }
    public int id_room { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public string video_type { get; set; }
    public int url_type { get; set; }
}
