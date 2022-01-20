using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class GameJsonParser : MonoBehaviour
{

      JsonData jsonData;
      string jsonString;

      public List<Information> infos = new List<Information>();
      
      private void Start()
      {
            Setting();

      }
      public void Setting()
      {
            if (PlayerPrefs.GetString("formation") == "attack")
            {
                  Debug.Log("Attack");
                  jsonString = File.ReadAllText(Application.persistentDataPath + "/Data/AttackFormation.json");
            }
            else if (PlayerPrefs.GetString("formation") == "defence")
            {
                  Debug.Log("Defence");
                  jsonString = File.ReadAllText(Application.persistentDataPath + "/Data/DefenceFormation.json");
            }
            else if (PlayerPrefs.GetString("formation") == "util")
            {
                  Debug.Log("Util");
                  jsonString = File.ReadAllText(Application.persistentDataPath + "/Data/UtilFormation.json");
            }

            jsonData = JsonMapper.ToObject(jsonString);

            for (int i = 0; i < jsonData.Count; i++)
            {
                  Debug.Log("i : " + i + " === " + (int)jsonData[i]["position_number"] + " , " + (int)jsonData[i]["demon_index"]);

            }
            for (int i = 0; i < jsonData.Count; i++)
            {

                  Information info = new Information((int)jsonData[i]["position_number"], (int)jsonData[i]["demon_index"]);
                  infos.Add(info);
            }
      }
}
