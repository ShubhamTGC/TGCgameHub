using System.Collections.Generic;

public class ActionPlanModel 
{
    public string Name { get; set; }
    public string Class { get; set; }
    public List<ActionPlanLeaderBoardList> LeaderList { get; set; }
}


public class ActionPlanLeaderBoardList
{
    public int Rank { get; set; }
    public string Name { get; set; }
    public double ActionPlanPoints { get; set; }
    public int ActionPlanTitle { get; set; }

}
