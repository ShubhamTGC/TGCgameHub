using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnagramModel
{
    public int id_word { get; set; }
    public int id_sheet { get; set; }
    public string question { get; set; }
    public string answer { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
}

public class Model
{
    public List<AnagramModel> AnagramArray { get; set; }

}
