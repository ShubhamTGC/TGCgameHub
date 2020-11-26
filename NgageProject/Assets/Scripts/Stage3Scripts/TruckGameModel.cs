using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;

public class TruckGameModel 
{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public string Truckname { get; set; }
    public int dustbinCollected { get; set; }
    public string Reachedcentername { get; set; }
    public int TruckScore { get; set; }
    public int CenterScore { get; set; }
    public int is_correctReached    { get; set; }

}
