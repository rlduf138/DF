using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class StartingCardController : Singleton<StartingCardController>
{
      public List<StartingCard> cardList;
      public List<bool> selectedCardBool;

      public List<StartingCard> selectList;

      public List<GameObject> startingList;           // 스타팅카드 리스트
      public List<GameObject> allMagicList;           // 전체 카드 리스트

      JsonData deckData;

      void Start()
      {
            SetStartingDeck();
      }

      void Update()
      {

      }

      public void SetStartingDeck()
      {
            Debug.Log("SetStartingDeck");
            string deckString = File.ReadAllText(Application.persistentDataPath + "/Data/StartingDeck.json");
            deckData = JsonMapper.ToObject(deckString);

            for(int i = 0; i<deckData.Count; i++)
            {
                  for (int k = 0; k < (int)deckData[i]["_count"]; k++)
                  {
                        Debug.Log(" magicList.Add(allMagicList[i]) i : " + i);
                        startingList.Add(allMagicList[i]);
                  }
            }

            for(int i = 0; i< 10; i++)
            {
                  cardList[i].SetMagic(startingList[i]);
            }
      }


      public void SelectCard(int n)
      {
            selectedCardBool[n] = true;
      }
      public void ReleaseCard(int n)
      {
            selectedCardBool[n] = false;
      }

      public void CancelButton()
      {
            gameObject.SetActive(false);
            SeaController.Instance.OnGgasi01();
      }
      public void CheckSelectList()
      {
            // 확인 버튼 클릭 시
            int count = 0;
            for(int i = 0; i<10; i++)
            {
                  if(selectedCardBool[i] == true)
                  {
                        count++;
                  }
            }
            if(count >= 7)
            {
                  SetSelectList();
                  SeaController.Instance.StartCoroutine("ActiveScene");
            }
            else
            {
                  Debug.LogWarning("선택 카드가 7장 미만");
            }
      }

      
      public void SetSelectList()
      {
            for(int i = 0; i<10; i++)
            {
                  if(selectedCardBool[i] == true)
                  {
                        selectList.Add(cardList[i]);
                  }
            }
            SaveDeck();
      }


      public void SaveDeck()
      {
            int count = 0;

            List<DeckList> list = new List<DeckList>();
            // 덱 json 설정
            DeckList deck = new DeckList(0, 0); list.Add(deck);
            deck = new DeckList(1, 0); list.Add(deck);
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



           
            // 덱 카운트 설정
            // cardobject -> index..
            for (int i = 0; i < selectList.Count; i++)
            {
                  int index = selectList[i].GetComponent<StartingCard>().magic_index;
                  list[index]._count++;
                  count++;
            }
            PlayerPrefs.SetInt("DeckCount", count);
            string saveData = JsonMapper.ToJson(list);
            File.WriteAllText(Application.persistentDataPath + "/Data/DeckList.json", saveData.ToString());
      }
}

class DeckList
{
      public int index;
      public int _count;

      public DeckList(int index, int _count)
      {
            this.index = index;
            this._count = _count;
      }
}
