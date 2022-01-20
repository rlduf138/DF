using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;

public class CardSelectController : Singleton<CardSelectController>
{
      public FadeController fadeController;

      [Header("Prefabs")]
      public GameObject cardObject;
      public GameObject attackDeckObject;
      public GameObject utilDeckObject;

      public GameObject cardContent;
      public Transform cardPos;
      public GameObject deckContent;
      public Transform deckPos;

      [Header("List")]
      public List<GameObject> cardList;
      public List<GameObject> deckList;

      JsonData hasData;
      JsonData infoData;
      JsonData deckData;

      public int deckCount = 0;
      public Text deckText;
      public int maxCount;

      // Start is called before the first frame update
      void Start()
      {
            fadeController.gameObject.SetActive(true);
            fadeController.FadeOut(1f, () => {
                  fadeController.gameObject.SetActive(false);
            });

            string hasString = File.ReadAllText(Application.persistentDataPath + "/Data/HasCard.json");
            hasData = JsonMapper.ToObject(hasString);
            
            
            string infoString = File.ReadAllText(Application.persistentDataPath + "/Data/CardInfo.json");
            infoData = JsonMapper.ToObject(infoString);

            CreateCard();

            if (File.Exists(Application.persistentDataPath + "/Data/DeckList.json"))
            {
                  string deckString = File.ReadAllText(Application.persistentDataPath + "/Data/DeckList.json");
                  deckData = JsonMapper.ToObject(deckString);

                  LoadDeck();
            }

            Debug.Log(Application.persistentDataPath);
            Debug.Log(Application.dataPath);
            Debug.Log(Resources.Load<Sprite>("Magic/Portrait/magicCardIllust_06.png"));
           
            deckText.text = "덱 목록  " + deckCount + " / " + maxCount;
      }


      public void LoadDeck()
      {
            // DeckList json -> deckData 에서 데이터 가져옴.
            // DeckData : index , _count
            for (int i = 0; i < deckData.Count; i++)
            {
                  if ((int)deckData[i]["_count"] != 0)
                  {     // 해당 인덱스 카드가 한장이라도 있으면.

                        for (int k = 0; k < (int)deckData[i]["_count"]; k++)
                        {
                              // _count만큼 반복.
                              //  cardList -> getComponent Card_Cs
                              GameObject deckObject;

                              for(int d = 0; d < infoData.Count; d++)
                              {
                                    if((int)infoData[d]["index"] == i)
                                    {
                                          Debug.Log("d : " + k);
                                          if ((int)infoData[d]["_type"] == 0)
                                          {     // 공격형.

                                                for (int j = 0; j < cardList.Count; j++)
                                                {
                                                      Card_CS _card = cardList[j].GetComponent<Card_CS>();
                                                      if (_card.IsDeck == false)
                                                      {
                                                            if (_card.index == (int)deckData[i]["index"])
                                                            {
                                                                  deckObject = Instantiate(attackDeckObject, deckContent.transform);

                                                                  deckObject.GetComponent<DeckButton>().cardObject = cardList[j];
                                                                  deckList.Add(deckObject);
                                                                  _card.IsDeck = true;
                                                                  _card.deckObject = deckObject;
                                                                  deckCount++;
                                                                  break;
                                                            }
                                                      }
                                                }
                                          }
                                          else if ((int)infoData[d]["_type"] == 1)
                                          {     // 유틸형

                                                for (int j = 0; j < cardList.Count; j++)
                                                {
                                                      Card_CS _card = cardList[j].GetComponent<Card_CS>();
                                                      if (_card.IsDeck == false)
                                                      {
                                                            if (_card.index == (int)deckData[i]["index"])
                                                            {
                                                                  deckObject = Instantiate(utilDeckObject, deckContent.transform);

                                                                  deckObject.GetComponent<DeckButton>().cardObject = cardList[j];
                                                                  deckList.Add(deckObject);
                                                                  _card.IsDeck = true;
                                                                  _card.deckObject = deckObject;
                                                                  deckCount++;
                                                                  break;
                                                            }
                                                      }
                                                }
                                          }
                                    }
                              }
                           
                        }
                  }
            }
            RefreshDeckList();
      }
      public void AddDeck(GameObject gameObject)
      {
            // 덱 최대치 제한 필요
            // 덱 리스트에 추가
            // y : -14 ,-10
            if (deckCount < maxCount)
            {
                  Card_CS _card = gameObject.GetComponent<Card_CS>();
                  GameObject deckObject;
                  if (_card.type == 0)
                  {
                        // 공격형
                        deckObject = Instantiate(attackDeckObject, deckContent.transform);
                        _card.IsDeck = true;

                        deckObject.GetComponent<DeckButton>().cardObject = gameObject;
                        deckList.Add(deckObject);
                        _card.deckObject = deckObject;
                  }
                  else if (_card.type == 1)
                  {
                        // 지원형
                        deckObject = Instantiate(utilDeckObject, deckContent.transform);
                        _card.IsDeck = true;

                        deckObject.GetComponent<DeckButton>().cardObject = gameObject;
                        deckList.Add(deckObject);
                        _card.deckObject = deckObject;
                  }

                  deckCount++;
                  deckText.text = "덱 목록  " + deckCount + " / " + maxCount;
                  RefreshDeckList();
            }
      }
      public void DeleteDeck(GameObject gameObject)
      {
            // 덱 삭제.
            Card_CS _card = gameObject.GetComponent<Card_CS>();
            for(int i  = 0; i<deckList.Count; i++)
            {
                  if(deckList[i] == _card.deckObject)
                  {
                        deckList.RemoveAt(i);
                        break;
                  }
            }
            deckCount--;
            Destroy(_card.deckObject);
            _card.IsDeck = false;
            _card.RefreshEdge();
            RefreshDeckList();

      }

      public void RefreshDeckList()
      {
            // 덱 정렬 및 Content 사이즈 조절
            RectTransform rectTransform = deckContent.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);

            for(int i = 0; i<deckList.Count; i++)
            {
                  rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y +25);
                  float x = deckPos.localPosition.x;
                  float y = deckPos.localPosition.y - 25 * (i);

                  deckList[i].transform.localPosition = new Vector2(x, y);
            }
            deckText.text = "덱 목록  " + deckCount + " / " + maxCount;
      }

      public void CreateCard()
      {
            // 카드리스트 추가  가로 5개
            // x : -200 ~ 200, +100
            // y : -74 -224 , -150

            int card_count = 0;

            // json에서 가져온 데이터 정리.
            for(int i = 0; i<hasData.Count; i++)
            {
                  for(int k = 0; k < (int)hasData[i]["_count"]; k++)
                  {
                        // 카드가 한장이상 있으면.
                        //Debug.Log("hasData : " + (int)hasData[i]["_count"]);
                        int a = (int)hasData[i]["index"];
                        
                        GameObject gameObject = Instantiate(cardObject, cardContent.transform);

                        Card_CS card_cs =  gameObject.GetComponent<Card_CS>();
                        for(int d = 0; d<infoData.Count; d++)
                        {
                              if(a == (int)infoData[d]["index"])
                              {
                                    Debug.Log("d : " + d);
                                    card_cs.index = (int)infoData[d]["index"];
                                    card_cs.card_name = (string)infoData[d]["_name"];
                                    card_cs.card_explain = (string)infoData[d]["_explain"];
                                    card_cs.cost = (int)infoData[d]["_cost"];
                                    card_cs.type = (int)infoData[d]["_type"];
                                    card_cs.card_sprite = Resources.Load<Sprite>("Magic/Portrait/" + (string)infoData[d]["_sprite"]);
                              }
                        }
                        /*card_cs.index = (int)infoData[a]["index"];
                        card_cs.card_name = (string)infoData[a]["_name"];
                        card_cs.card_explain = (string)infoData[a]["_explain"];
                        card_cs.cost = (int)infoData[a]["_cost"];
                        card_cs.type = (int)infoData[a]["_type"];
                        card_cs.card_sprite = Resources.Load<Sprite>("Magic/Portrait/" + (string)infoData[a]["_sprite"]);
                        */
                        float x = cardPos.localPosition.x + 100 * (card_count % 5);
                        float y = cardPos.localPosition.y - 150 * (card_count / 5);

                        gameObject.transform.localPosition = new Vector2(x, y);

                        cardList.Add(gameObject);
                        card_count++;

                        // content 세로 길이 늘리기
                        // 기존 height = 160
                        if (card_count%5 == 0)
                        {
                              RectTransform rectTransform = cardContent.GetComponent<RectTransform>();
                              rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + 150);
                        }
                  }
            }
      }
      
      public void SaveDeck()
      {
            int count = 0;

            List<DeckList> list = new List<DeckList>();
            // 덱 json 설정
            DeckList deck = new DeckList(0, 0); list.Add(deck);
            deck = new DeckList(1, 0);    list.Add(deck);
            deck = new DeckList(2, 0); list.Add(deck);
            deck = new DeckList(3, 0); list.Add(deck);
            deck = new DeckList(4, 0); list.Add(deck);
            deck = new DeckList(5, 0); list.Add(deck);
            deck = new DeckList(6, 0); list.Add(deck);
            deck = new DeckList(7, 0); list.Add(deck);
            deck = new DeckList(8, 0); list.Add(deck);
            deck = new DeckList(9, 0); list.Add(deck);
            deck = new DeckList(10, 0); list.Add(deck);
            deck = new DeckList(11, 0); list.Add(deck);
            deck = new DeckList(12, 0); list.Add(deck);
            deck = new DeckList(13, 0); list.Add(deck);
            deck = new DeckList(14, 0); list.Add(deck);
            deck = new DeckList(15, 0); list.Add(deck);
            deck = new DeckList(16, 0); list.Add(deck);
            deck = new DeckList(17, 0); list.Add(deck);
            deck = new DeckList(18, 0); list.Add(deck);
            deck = new DeckList(19, 0); list.Add(deck);
            deck = new DeckList(20, 0); list.Add(deck);
            deck = new DeckList(21, 0); list.Add(deck);
            deck = new DeckList(22, 0); list.Add(deck);
            deck = new DeckList(23, 0); list.Add(deck);
            deck = new DeckList(24, 0); list.Add(deck);
            deck = new DeckList(25, 0); list.Add(deck);
            deck = new DeckList(26, 0); list.Add(deck);
            deck = new DeckList(27, 0); list.Add(deck);
            deck = new DeckList(28, 0); list.Add(deck);
            deck = new DeckList(29, 0); list.Add(deck);
            deck = new DeckList(30, 0); list.Add(deck);
            deck = new DeckList(31, 0); list.Add(deck);
            deck = new DeckList(32, 0); list.Add(deck);
            deck = new DeckList(33, 0); list.Add(deck);
            deck = new DeckList(34, 0); list.Add(deck);
            deck = new DeckList(35, 0); list.Add(deck);
            deck = new DeckList(36, 0); list.Add(deck);
            deck = new DeckList(37, 0); list.Add(deck);
            deck = new DeckList(38, 0); list.Add(deck);
            deck = new DeckList(39, 0); list.Add(deck);
            deck = new DeckList(40, 0); list.Add(deck);
            deck = new DeckList(41, 0); list.Add(deck);
            deck = new DeckList(42, 0); list.Add(deck);
            deck = new DeckList(43, 0); list.Add(deck);
            deck = new DeckList(44, 0); list.Add(deck);
            deck = new DeckList(45, 0); list.Add(deck);
            deck = new DeckList(46, 0); list.Add(deck);
            deck = new DeckList(47, 0); list.Add(deck);



            /*   DeckList card00 = new DeckList(0, 0);
               DeckList card01 = new DeckList(1, 0);
               DeckList card02 = new DeckList(2, 0);
               DeckList card03 = new DeckList(3, 0);
               DeckList card04 = new DeckList(4, 0);
               DeckList card05 = new DeckList(5, 0);
               DeckList card06 = new DeckList(6, 0);
               DeckList card07 = new DeckList(7, 0);
               DeckList card08 = new DeckList(8, 0);
               list.Add(card00); list.Add(card01); list.Add(card02); list.Add(card03); list.Add(card04);
               list.Add(card05); list.Add(card06); list.Add(card07); list.Add(card08);
               */
            // 덱 카운트 설정
            // cardobject -> index..
            for (int i = 0; i<deckList.Count; i++)
            {
                  int index = deckList[i].GetComponent<DeckButton>().cardObject.GetComponent<Card_CS>().index;
                  list[index]._count++;
                  count++;
            }
            PlayerPrefs.SetInt("DeckCount", count);
            string saveData = JsonMapper.ToJson(list);
            File.WriteAllText(Application.persistentDataPath + "/Data/DeckList.json", saveData.ToString());
      }
      public void BackButtonClick()
      {
            SaveDeck();
            fadeController.gameObject.SetActive(true);
            fadeController.FadeIn(1f, () =>
            {
                  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StageSelect");
            });
      }
}
/*class DeckList
{
      public int index;
      public int _count;

      public DeckList(int index, int _count)
      {
            this.index = index;
            this._count = _count;
      }
}*/
