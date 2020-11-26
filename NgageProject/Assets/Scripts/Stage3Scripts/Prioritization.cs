using SimpleSQL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prioritization

{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public string Truckname { get; set; }
    public int Is_correct { get; set; }
    public int Truckscore { get; set; }
    public string CorrectTruck { get; set; }

}
