
using System;
using System.Collections.Generic;

public class GameSetupModel 
{
    public List<Gamelist> gamelist { get; set; }
    public List<Anagramlist> anagramlist { get; set; }
    public List<Monstercatchlist> monstercatchlist { get; set; }
    public List<Truckgamelist> truckgamelist { get; set; }
    public List<Quizelist> quizelist { get; set; }
    public List<Objectgamilist> objectgamilist { get; set; }
    public List<Truckscoretypelist> truckscoretypelist { get; set; }
    public List<Truckdestinationdrivinglist> truckdestinationdrivinglist { get; set; }
    public List<matchthetiletypelist> matchthetiletypelist { get; set; }
    public List<matchthetilelist> matchthetilelist { get; set; }

}

public class Gamelist
{
    public int Id_Game { get; set; }
    public string GameName { get; set; }
    public int ID_ORGANIZATION { get; set; }
    public string IsActive { get; set; }
    public DateTime Updated_Date_Time { get; set; }
    public string GameType { get; set; }
    public string completePer { get; set; }
    public string RectangleImgURL { get; set; }
    public string RoundedImgURL { get; set; }

    public int UpdatedFlag { get; set; }
}

public class Anagramlist
{
    public int id_word { get; set; }
    public int Id_Game { get; set; }
    public string question { get; set; }
    public string answer { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public int? anagram_type { get; set; }
    public string Image { get; set; }
    public string Path { get; set; }
    public int correct_point { get; set; }
    public int Wrong_point { get; set; }
    public int ID_ORGANIZATION { get; set; }
    public string BackgrounImageURL { get; set; }
}

public class Monstercatchlist
{
    public int monsterId { get; set; }
    public int id_truck { get; set; }
    public int catch_point { get; set; }
    public int UpdatedFlag { get; set; }
    public int Id_Game { get; set; }
    public object tbl_truck_prioritize_master { get; set; }
}

public class Truckgamelist
{
    public int id_truck { get; set; }
    public int Id_Game { get; set; }
    public string GameName { get; set; }
    public string truck_name { get; set; }
    public int Dustbins_Map_Id { get; set; }
    public string Dustbins_Name { get; set; }
    public int Correct_Dustbin_Point { get; set; }
    public int Wrong_Dustbin_Point { get; set; }
    public int ScoreId { get; set; }
}

public class Quizelist
{
    public int Id_Quiz { get; set; }
    public int Id_Game { get; set; }
    public int Correct_point { get; set; }
    public int Wrong_point { get; set; }
    public string Question { get; set; }
    public string Correct_Options { get; set; }
    public string Options_1 { get; set; }
    public string Options_2 { get; set; }
    public string Options_3 { get; set; }
    public string IsActive { get; set; }
    public DateTime Updated_Date_Time { get; set; }
    public int ID_ORGANIZATION { get; set; }
}

public class Objectgamilist
{
    public int Id_Game { get; set; }
    public string Game_Name { get; set; }
    public int Id_Room { get; set; }
    public string Room_Name { get; set; }
    public int item_Id { get; set; }
    public string item_Name { get; set; }
    public int correct_point { get; set; }
    public int Wrong_point { get; set; }
}

public class Truckscoretypelist
{
    public int ScoreId { get; set; }
    public string ScoreType { get; set; }
}

public class Truckdestinationdrivinglist
{
    public int driveId { get; set; }
    public int id_truck { get; set; }
    public string destination_name { get; set; }
    public int correct_bonus_point { get; set; }
    public int wrong_point { get; set; }
    public int ScoreId { get; set; }
    public object tbl_truck_prioritize_master { get; set; }
}

public class matchthetiletypelist
{
    public int id_waste_type { get; set; }
    public string type { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public string ImageUrl { get; set; }
    public int Id_Game { get; set; }
    public int ID_ORGANIZATION { get; set; }
}

public class matchthetilelist
{
    public int id_waste { get; set; }
    public string waste { get; set; }
    public int waste_type { get; set; }
    public string status { get; set; }
    public DateTime updated_date_time { get; set; }
    public int Score { get; set; }
    public string ImageUrl { get; set; }
    public int Id_Game { get; set; }
    public int Wrong_point { get; set; }
    public int ID_ORGANIZATION { get; set; }

}


