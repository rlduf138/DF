using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class StartingDeck : MonoBehaviour
{
      public FadeController fadeController;
    // Start is called before the first frame update
    void Start()
    {
            fadeController.gameObject.SetActive(true);
            fadeController.FadeOut(1f, () => {
                  fadeController.gameObject.SetActive(false);
            });
            PlayerPrefs.SetInt("DeckCount", 0);
            CreateDeck();
            //CreateCardInfo();
    }

 
      public void StartButtonClick()
      {
            fadeController.gameObject.SetActive(true);
            fadeController.FadeIn(1f, () =>
            {
                  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StageSelect");
            });
      }
      private void CreateDeck()
      {
            if (!File.Exists(Application.persistentDataPath + "/Data/HasCard.json"))
            {
                  Debug.Log("HasCard Write");
                  // 첫 실행시 HasCard.json 생성.
                  List<HasCard> list = new List<HasCard>();

                  HasCard card = new HasCard(0, 0);
                  list.Add(card);
                  card = new HasCard(1, 0);     // Reattack은 assemble 에 포함.
                  list.Add(card);
                  card = new HasCard(2, 0);
                  list.Add(card);
                  card = new HasCard(3, 0);
                  list.Add(card);
                  card = new HasCard(4, 0);
                  list.Add(card);
                  card = new HasCard(5, 1);
                  list.Add(card);
                  card = new HasCard(6, 0);
                  list.Add(card);
                  card = new HasCard(7, 0);
                  list.Add(card);
                  card = new HasCard(8, 1);
                  list.Add(card);
                  card = new HasCard(9, 1);
                  list.Add(card);
                  card = new HasCard(10, 0);
                  list.Add(card);
                  card = new HasCard(11, 0);
                  list.Add(card);
                  card = new HasCard(12, 0);
                  list.Add(card);
                  card = new HasCard(13, 1);
                  list.Add(card);
                  card = new HasCard(14, 0);
                  list.Add(card);
                  card = new HasCard(15, 0);
                  list.Add(card);
                  card = new HasCard(16, 0);
                  list.Add(card);

                  card = new HasCard(24, 1);
                  list.Add(card);
                  card = new HasCard(25, 1);
                  list.Add(card);
                  card = new HasCard(26, 1);
                  list.Add(card);
                  card = new HasCard(27, 1);
                  list.Add(card);
                  card = new HasCard(28, 1);
                  list.Add(card);

                  card = new HasCard(35, 1);
                  list.Add(card);
                  card = new HasCard(38, 0);
                  list.Add(card);

                  card = new HasCard(47, 1);
                  list.Add(card);



                  /*HasCard card00 = new HasCard(5, 1);
                  HasCard card01 = new HasCard(8, 2);
                  HasCard card02 = new HasCard(9, 1);
                  HasCard card03 = new HasCard(10, 0);
                  HasCard card04 = new HasCard(11, 1);
                  HasCard card05 = new HasCard(12, 0);
                  HasCard card06 = new HasCard(6, 1);
                  HasCard card07 = new HasCard(7, 1);
                  HasCard card08 = new HasCard(8, 2);
                  list.Add(card00); list.Add(card01); list.Add(card02); list.Add(card03); list.Add(card04);
                  list.Add(card05); list.Add(card06); list.Add(card07); list.Add(card08);
                  */
                  string saveData = JsonMapper.ToJson(list);
                  File.WriteAllText(Application.persistentDataPath + "/Data/HasCard.json", saveData.ToString());
            }
      }
      
  
}
class HasCard
{
      public int index;
      public int _count;

     public HasCard(int index, int _count)
      {
            this.index = index;
            this._count = _count;
      }
}
class CardInfo
{
      public int index;
      public int _type;
      public string _name;
      public string _explain;
      public int _cost;
      public string _sprite;

      public CardInfo(int index, int _type, string _name, string _explain, int _cost, string _sprite)
      {
            this.index = index;
            this._type = _type;
            this._name = _name;
            this._explain = _explain;
            this._cost = _cost;
            this._sprite = _sprite;
      }
}
