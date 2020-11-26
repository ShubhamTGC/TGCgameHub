using SimpleSQL;

public class VideoUrls 
{
    [AutoIncrement,PrimaryKey]
    public int Id { get; set; }
    public int VideoId { get; set; }
    public string VideoLink { get; set; }
    public int LevelId { get; set; }
    public string VideoType { get; set; }
    public int UrlType { get; set; }

}
