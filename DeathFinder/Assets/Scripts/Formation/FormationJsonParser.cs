using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class FormationJsonParser : MonoBehaviour
{
      public Transform content;
      public GameObject[] demon_button;

      public Transform buttonPos;

      JsonData saveData;
      JsonData hasData;

      List<HasDemon> lists = new List<HasDemon>();

      // Start is called before the first frame update
      void Start()
      {

            
            if(Directory.Exists(Application.persistentDataPath + "/Data") == false)
            {
                  Directory.CreateDirectory(Application.persistentDataPath + "/Data");
            }

            SaveData();

            string hasString = File.ReadAllText(Application.persistentDataPath + "/Data/HasDemon.json");
            hasData = JsonMapper.ToObject(hasString);


            SettingDemon();
      }

      private void SettingDemon()
      {
            int j = 0;

            for(int i = 0; i < 11; i++)
            {
                  if ((bool)hasData[i]["_has"] == true)
                  {

                        GameObject gameObject = Instantiate(demon_button[i], content);
                        DemonButton db = gameObject.GetComponent<DemonButton>();

                        float x = buttonPos.localPosition.x + 42*(j%5);
                        float y = buttonPos.localPosition.y - 60*(j/5);

                        db.transform.localPosition = new Vector2(x, y);
                     //   Debug.Log("x : " + db.transform.position.x);
                        j++;
                  }
            }
      }

     private void CreateDemonButton(int i)
      {
            if ((bool)hasData[i]["_has"] == true)
            {

                  GameObject gameObject = Instantiate(demon_button[i], content);
                  DemonButton db = gameObject.GetComponent<DemonButton>();

                  //db.SetDemonButton(index, dName, aLevel, sLevel, rLevel);
            }
      }

      public void SaveData()
      {
            // 데몬 소지 여부.
            for( int i = 0; i<11; i++)
            {
                  HasDemon hasDemon = new HasDemon(i, true);
                  lists.Add(hasDemon);
            }
            saveData = JsonMapper.ToJson(lists);
            // + "/Resource/JsonData/HasDemon.json"
            File.WriteAllText(Application.persistentDataPath + "/Data/HasDemon.json", saveData.ToString());
      }

      class HasDemon
      {
            public int index;
            public bool _has;

            public HasDemon(int _index, bool has)
            {
                  index = _index;
                  _has = has;
            }
      }
}
