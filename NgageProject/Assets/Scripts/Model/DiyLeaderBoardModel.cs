using System.Collections.Generic;

public class DiyLeaderBoardModel 
{
    public string Name { get; set; }
    public string Class { get; set; }
    public List<DIYLeaderBoardList> LeaderList { get; set; }
}

public class DIYLeaderBoardList
{
    public int Rank { get; set; }
    public string Name { get; set; }
    public double DIYPoints { get; set; }
    public int DIYTitle { get; set; }

}
