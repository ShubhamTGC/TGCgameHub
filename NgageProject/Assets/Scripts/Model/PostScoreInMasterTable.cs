using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class PostScoreInMasterTable  :MonoBehaviour
{
    string Mainurl = "https://www.skillmuni.in/wsmapi/api/";
    string UserScorePosting = "OrgGameScoreDataPost";
    bool response = false;

   
    public IEnumerator GameScorePosting(int Score,int Game_content,string Time_taken,int Completed,int attemptno,int id_level)
    {
        string hittingUrl = Mainurl + UserScorePosting;
        ScorePostModel postField = new ScorePostModel();
        postField.UID = PlayerPrefs.GetInt("UID");
        postField.OID = PlayerPrefs.GetInt("OID");
        postField.id_log = 1;
        postField.id_user = PlayerPrefs.GetInt("UID");
        postField.id_game_content = Game_content;
        postField.score = Score;
        postField.id_score_unit = 1;
        postField.score_type = 1;
        postField.score_unit = "points";
        postField.status = "A";
        postField.updated_date_time = DateTime.Now.ToString();
        postField.id_level = id_level;
        postField.id_org_game = 1;
        postField.attempt_no = attemptno+1;
        postField.timetaken_to_complete = Time_taken;
        postField.is_completed = Completed;
        postField.game_type = 2;


        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(postField);

        using (UnityWebRequest request = UnityWebRequest.Put(hittingUrl, PostLog))
        {
            Debug.Log(request);
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                Debug.Log(request.downloadHandler.text);

                JsonData post_res = JsonMapper.ToObject(request.downloadHandler.text);
                string authstatus = post_res["STATUS"].ToString();
                response = authstatus.Equals("success");
            }
        }

        
    }
}
