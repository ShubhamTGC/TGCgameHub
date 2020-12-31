using System;

public class ScratchCardHistoryModel
{
    public int id_log_scratchcard { get; set; }
    public int Id_User { get; set; }
    public int id_scratchcard { get; set; }
    public string scratchcard { get; set; }
    public int scratch_point { get; set; }
    public int ID_ORGANIZATION { get; set; }
    public string status { get; set; }
    public DateTime Updated_Date_Time { get; set; }
    public int? IsScratch { get; set; }
    public int Card_type { get; set; }
}
