using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBadgeDetails
{
    public List<BadgeDetails> BadgeInfo { get; set; }
}

public class BadgeDetails
{
    public int id_badge { get; set; }
    public string badge_name { get; set; }
    public int id_level { get; set; }
    public int badge_type { get; set; }
    public string status { get; set; }
    public int badge_eligibility_score { get; set; }
    public DateTime updated_date_time { get; set; }

}


