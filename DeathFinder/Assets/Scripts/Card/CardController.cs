using PixelSilo.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using LitJson;
using System.IO;
using TMPro;

public class CardController : Singleton<CardController>
{
      public BattleData data;
      public TextMeshProUGUI chaosText;

      public Button deckButton;
      public Slider deckSlider;

      public GameObject card_prefab;

      public int maxCardCount;

      
      public Transform[] card_transfomrs;
      public GameObject particleParent;
      public bool _sorting;
      public bool _adding;
      public bool assembleFlag;     // true : assemble사용시 다음카드 reattack

      [Header("Except_Card")]
      public List<GameObject> except_cards;

      [Header("Panel")]
      public GameObject cancel_panel;
      public GameObject summon_panel;

      [Header("Cards")]
      public List<GameObject> cards;
      public bool[] card_enable;

      [Header("TouchArea")]
      public List<GameObject> touchAreas;

      [Header("MagicCardList")]
      public List<GameObject> allMagicList;
      public List<GameObject> originDeck;
      public List<GameObject> copyDeck;
      PettyGreed pettyGreed;

      JsonData deckData;
      private float deck_current_time;

      private void Awake()
      {
            SetMagicList();
            CopyShuffleDeckList();
      }

      void Start()
      {
            //ShuffleAllCard();
            //  CopyShuffleDeckList();

            //  cardSlider.maxValue = data._turnPeriod;
            //  originalCardTime = data._turnPeriod;
            deckSlider.maxValue = data._deckPeriod;
            StartCoroutine(DeckTimer());
            
      }

      public void SetMagicList()
      {
            Debug.Log("SetMagicList");
            string deckString = File.ReadAllText(Application.persistentDataPath + "/Data/DeckList.json");
            deckData = JsonMapper.ToObject(deckString);
            Debug.Log("Json Mapper");

            for (int i = 0; i < deckData.Count; i++)
            {
                  for (int k = 0; k < (int)deckData[i]["_count"]; k++)
                  {
                        Debug.Log(" magicList.Add(allMagicList[i]) i : " + i);
                        originDeck.Add(allMagicList[i]);
                  }
            }
            Debug.Log("SetMagicList End");
      }
      public void CopyShuffleDeckList()
      {
            // 원본덱을 복사.
            Debug.Log("CopyMagicList");
            copyDeck = new List<GameObject>();
            for (int i = 0; i < originDeck.Count; i++)
            {
                  GameObject gameObject = originDeck[i];
                  copyDeck.Add(gameObject);
            }
            copyDeck = ShuffleCopyDeck(copyDeck);
            data._stackChaos++;
            ChaosStackRefresh();
      }
      public void ChaosStackRefresh()
      {
            chaosText.text = "Chaos : " + data._stackChaos;
      }

      public List<GameObject> ShuffleCopyDeck(List<GameObject> inputList)
      {
            // 덱 섞기
            Debug.Log("ShuffleCopyDeck");

            List<GameObject> randomList = new List<GameObject>();

            System.Random r = new System.Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                  randomIndex = r.Next(0, inputList.Count);
                  randomList.Add(inputList[randomIndex]);
                  inputList.RemoveAt(randomIndex);
            }

            return randomList;

      }

      public IEnumerator CardOffOn(int card_number)
      {
            Debug.Log("CardOffOn  Cardnumber : " + card_number);
            cards[card_number].GetComponent<Card>().GotoPrevPosition();
            cards[card_number].SetActive(false);

            yield return new WaitForSeconds(0.05f);
            if (card_enable[card_number] == true)
            {
                  cards[card_number].SetActive(true);
            }
            else
            {
                  Debug.Log("Cardenable false : " + card_number);
            }

      }

      public void OnDeckButtonClicked()
      {
            Debug.Log("OnDeckButtonClick");
            if (card_enable[0] == true)
            {
                  return;
            }

            StartCoroutine("AddCard");
            deck_current_time = 0;
            deckSlider.value = deck_current_time;
            deckButton.gameObject.SetActive(false);
      }
      public IEnumerator DeckTimer()
      {
            deck_current_time = 0;
            deckSlider.value = deck_current_time;
            deckButton.gameObject.SetActive(false);
            while (true)
            {

                  while (deck_current_time < data._deckPeriod)
                  {
                        yield return new WaitForSeconds(0.1f);
                        deck_current_time += 0.1f;
                        deckSlider.value = deck_current_time;
                        if(deck_current_time >=  data._deckPeriod)
                        {
                              deckButton.gameObject.SetActive(true);
                        }
                        //draw_text.text = string.Format("{0:F1}",current_time);
                  }
                  yield return new WaitForSeconds(0.1f);
                  
            }
      }


      public IEnumerator Sorting(int card_number)
      {
            Debug.Log("Sorting");

            while (_adding == true)
            {
                  // addcard 진행중이라면.
                  yield return new WaitForSeconds(0.05f);
            }
            _sorting = true;  // sorting 진행중임.

            PanelsOn();

            int false_count = 0;
            for (int i = 0; i < maxCardCount; i++)
            {
                  if (card_enable[i] == false)
                  {
                        false_count++;
                  }
            }

            Destroy(cards[card_number]);

            for (int i = card_number; i > 0; i--)
            {
                  //   Debug.Log("앞제거 i : " + i);
                  if (card_enable[i - 1] != false)
                  {
                        cards[i - 1].GetComponent<Card>().StartCoroutine("CardSorting", card_transfomrs[i]);
                  }

                  yield return new WaitForSeconds(0.09f);
                  if (card_enable[i - 1] == false)
                  {
                        card_enable[i] = false;
                        break;
                  }
            }

            for (int i = card_number - 1; i >= 0; i--)
            {
                  if (cards[i] != null)
                  {
                        cards[i].GetComponent<Card>().card_number = i + 1;
                  }
                  cards[i + 1] = cards[i];
            }
            cards[0] = null;
            card_enable[0] = false;

            // touch area  활성화.
            for (int i = 0; i < cards.Count; i++)
            {
                  if (cards[i] == null)
                  {
                        touchAreas[i].GetComponent<CardTouch>().card = null;
                        touchAreas[i].SetActive(false);
                  }
                  else
                  {
                        cards[i].GetComponent<Card>().card_number = i;
                        touchAreas[i].GetComponent<CardTouch>().card = cards[i].GetComponent<Card>();
                  }
            }

            PanelsOff();
            //yield return new WaitForSeconds(0.1f);
            _sorting = false;
            Debug.Log("Sorting End");
      }


      public IEnumerator AddCard()
      {

            Debug.Log("AddCard Coroutine Start");

            for (int i = maxCardCount - 1; i >= 0; i--)
            {
                  if (card_enable[i] == false)
                  {
                        Debug.Log(i + " CardAdd");
                        GameObject card_object = Instantiate(card_prefab, card_transfomrs[i], false);
                        card_object.SetActive(false);
                        StartCoroutine(ParticleCard(i, card_object));

                        card_object.GetComponent<Card>().transform.localPosition = new Vector2(0, 0);
                        card_object.GetComponent<Card>().card_number = i;
                        cards.RemoveAt(i);
                        cards.Insert(i, card_object);

                        // 터치영역 세팅
                        touchAreas[i].GetComponent<CardTouch>().card = card_object.GetComponent<Card>();
                        touchAreas[i].SetActive(true);
                        ShuffleChance(i);

                        card_enable[i] = true;
                        //cards[i].SetActive(true);
                        //  break;
                  }
            }

            Debug.Log("AddCard End");
            yield return null;

           
      }

      public IEnumerator DoubleCardStart()
      {
            Debug.Log("DoubleCard Start");
            /*
                        while (_sorting == true || _adding == true)
                        {
                              //sorting 진행중이면.
                              yield return new WaitForSeconds(0.05f);
                        }
                        _adding = true;

                        bool flag = false;
                        int count = 0;
                        for (int i = 9; i >= 0; i--)
                        {
                              if (card_enable[i] == false && count <= 1)
                              {
                                    count++;
                                    Debug.Log("CardAdded");
                                    // new code
                                    GameObject card_object = Instantiate(card_prefab, card_transfomrs[i], false);
                                    card_object.SetActive(false);
                                    StartCoroutine(ParticleCard(i, card_object));

                                    card_object.GetComponent<Card>().transform.localPosition = new Vector2(0, 0);
                                    card_object.GetComponent<Card>().card_number = i;
                                    cards.RemoveAt(i);
                                    cards.Insert(i, card_object);

                                    // 터치영역 세팅
                                    touchAreas[i].GetComponent<CardTouch>().card = card_object.GetComponent<Card>();
                                    touchAreas[i].SetActive(true);

                                    ShuffleChance(i);

                                    flag = true;
                                    card_enable[i] = true;
                                    //cards[i].SetActive(true);
                                    //break;
                              }
                        }
                        if (flag == false)
                        {
                              _adding = false;
                        }
                        Debug.Log("DoubleCard End");
            */
            yield return null;

      }

      public IEnumerator ParticleCard(int i, GameObject gameObject)
      {
            GameObject particle = Instantiate(particleParent, deckButton.transform.position, Quaternion.identity);

            Vector3 _nextPosition = new Vector3(card_transfomrs[i].position.x, card_transfomrs[i].position.y + 1, card_transfomrs[i].position.z);
            //Vector3 _nextPosition = card_transfomrs[i].position;
            card_enable[i] = true;
            while (particle.transform.position.x < card_transfomrs[i].position.x)
            {
                  yield return new WaitForFixedUpdate();

                  Vector3 direction = _nextPosition - particle.transform.position;
                  direction.Normalize();

                  particle.transform.transform.Translate(direction * 30f * Time.deltaTime);
            }
            gameObject.SetActive(true);
            // gameObject.GetComponent<Card>().StartCoroutine("EdgeEffect");
            gameObject.GetComponent<Card>().CheckEdge();
            Destroy(particle);
            _adding = false;
      }


      public void PanelsOn()
      {
            cancel_panel.SetActive(true);
            summon_panel.SetActive(true);
      }
      public void PanelsOff()
      {
            cancel_panel.SetActive(false);
            summon_panel.SetActive(false);
      }

      public void ShuffleAllCard()
      {
            Debug.Log("ShuffleAllCard");
            for (int i = 0; i < maxCardCount; i++)
            {
                  card_enable[i] = true;
            }
            for (int i = 0; i < maxCardCount; i++)
            {
                  cards[i].SetActive(true);
                  ShuffleChance(i);
            }

      }

      public void ShuffleChance(int i)
      {
            PickCardOnDeck(i);
      }

      public void PickCardOnDeck(int i)
      {
            Debug.Log("PickCardOnDeck");
            if (assembleFlag == true)
            {
                  // 다음 카드  Reattack 소환.
                  cards[i].GetComponent<Card>().SetMagic(allMagicList[1]);
                  assembleFlag = false;
            }
            else if (assembleFlag == false)
            {
                  if (copyDeck.Count >= 1)
                  {
                        cards[i].GetComponent<Card>().SetMagic(copyDeck[0]);
                        copyDeck.RemoveAt(0);
                  }
                  else
                  {
                        Debug.Log("덱 카드 부족");
                        CopyShuffleDeckList();
                        PickCardOnDeck(i);
                  }
            }

      }


      public bool CheckPossible(GameObject cardObj)
      {
            if (cardObj.GetComponent<Berserker>() != null)
            {
                  if (FindObjectOfType<Berserker>() == null)
                  {
                        return true;
                  }
                  else
                  {
                        FindObjectOfType<Berserker>().current_time = 0f;
                        return true;
                  }
            }
            else if (cardObj.GetComponent<Barrier>() != null)
            {
                  if (FindObjectOfType<Barrier>() == null)
                  {
                        return true;
                  }
                  else
                  {
                        FindObjectOfType<Barrier>().current_time = 0f;
                        return true;
                  }
            }
            else if (cardObj.GetComponent<Revival>() != null)
            {
                  if (GameController.Instance.RevivalCheck())
                  {
                        return true;
                  }
                  else
                        return false;
            }
            else if (cardObj.GetComponent<Weakness>() != null)
            {
                  if (FindObjectOfType<Weakness>() == null)
                  {
                        return true;
                  }
                  else
                  {
                        FindObjectOfType<Weakness>().current_time = 0f;
                        return true;
                  }
            }
            else if (cardObj.GetComponent<Freeze>() != null)
            {
                  if (FindObjectOfType<Freeze>() == null)
                  {
                        return true;
                  }
                  else
                  {
                        FindObjectOfType<Freeze>().current_time = 0f;
                        return true;
                  }
            }
            else if (cardObj.GetComponent<Condemn>() != null)
            {
                  if (FindObjectOfType<Condemn>() == null)
                  {
                        return true;
                  }
                  else
                  {
                        FindObjectOfType<Condemn>().current_time = 0f;
                        return true;
                  }
            }
            else if (cardObj.GetComponent<ManaEater>() != null)
            {
                  if (data._currentMana == data._maxMana + data._maxManaLevel)
                  {
                        return false;
                  }
                  else
                        return true;
            }
            else if (cardObj.GetComponent<PettyGreed>() != null)
            {
                  if (FindObjectOfType<PettyGreed>() == null)
                  {
                        return true;
                  }
                  else
                  {
                        FindObjectOfType<PettyGreed>().current_count = 0;
                        return true;
                  }
            }
            else if (cardObj.GetComponent<FieldHospital>() != null)
            {
                  if (FindObjectOfType<FieldHospital>() == null)
                  {
                        return true;
                  }
                  else
                  {
                        Destroy(FindObjectOfType<FieldHospital>().gameObject);
                        return true;
                  }
            }


            return true;
      }


      public void PettyGreed(int count, float percent)
      {
            /*    pettyGreed = FindObjectOfType<PettyGreed>();
                cardAddTime = cardAddTime - (cardAddTime * percent / 100);
                cardSlider.maxValue = cardAddTime;*/
      }
      public void PettyGreedEnd()
      {
            /*   cardAddTime = originalCardTime;
               cardSlider.maxValue = cardAddTime;*/
      }

      /*  public void ShuffleMagic(int i)
   {

               if (assembleFlag == true)
               {
                     // 다음 카드  Reattack 소환.
                     cards[i].GetComponent<Card>().SetMagic(allMagicList[1]);
                     assembleFlag = false;

               }
               else if (assembleFlag == false)
               {
                     int ran = Random.Range(0, originDeck.Count);
                     bool revival_flag = false;
                     // 부활카드 2장이상 안되기위해서..
                     for(int j = 0; j< cards.Count; j++)
                     {
                           if (cards[j] != null)
                           {
                                 if (cards[j].GetComponent<Card>().magic_prefab == allMagicList[10])
                                 {
                                       revival_flag = true;
                                 }
                           }
                     }
                     if(revival_flag == true)
                     {
                           if (originDeck[ran] == allMagicList[10])
                           {
                                 Debug.Log("부활 중복");
                                 ShuffleMagic( i);
                           }
                           else
                           {
                                 cards[i].GetComponent<Card>().SetMagic(originDeck[ran]);
                           }
                     }
                     else
                     {
                           cards[i].GetComponent<Card>().SetMagic(originDeck[ran]);
                     }

               }


   }*/


      /*public void AddCard2()
      {
            // 4번카드가 없으면 4번에 소환.
            // 4번이 제일 마지막에 없어지기 때문.

            int false_count = 0;
            for (int i = 0; i < 8; i++)
            {
                  if (card_enable[i] == false)
                  {
                        false_count++;
                  }
            }
            if (false_count == 0)
            {
                  // 빈 공간이 없으면 return
                  return;
            }

            if(card_enable[4] == false)
            {
                  card_enable[4] = true;
                  GameObject card_object = Instantiate(card_prefab, card_transfomrs[4], false);
                  card_object.GetComponent<Card>().transform.localPosition = new Vector2(0, 0);
                  card_object.GetComponent<Card>().card_number = 4;
                  cards.RemoveAt(4);
                  cards.Insert(4, card_object);
                  touchAreas[4].GetComponent<CardTouch>().card = card_object.GetComponent<Card>();
                  touchAreas[4].SetActive(true);

                  ShuffleChance(4);
                  return;
            }

            if (false_count % 2 == 1)
            {
                  // 앞에 추가
                  for(int i = 4; i>=0; i--)
                  {
                        if(card_enable[i] == false)
                        {
                              // new code
                              GameObject card_object = Instantiate(card_prefab, card_transfomrs[i], false);
                              card_object.GetComponent<Card>().transform.localPosition = new Vector2(0, 0);
                              card_object.GetComponent<Card>().card_number = i;
                              cards.RemoveAt(i);
                              cards.Insert(i, card_object);

                              // 터치영역 세팅
                              touchAreas[i].GetComponent<CardTouch>().card = card_object.GetComponent<Card>();
                              touchAreas[i].SetActive(true);

                              ShuffleChance(i);

                              card_enable[i] = true;
                              cards[i].SetActive(true);
                              return;
                        }
                  }
            }
            else
            {
                  // 뒤에 추가
                  for (int i = 4; i < 8; i++)
                  {
                        if (card_enable[i] == false)
                        {
                              // new code
                              Debug.Log("add i : " + i);
                              GameObject card_object = Instantiate(card_prefab, card_transfomrs[i], false);
                              card_object.GetComponent<Card>().transform.localPosition = new Vector2(0, 0);

                              //cards.
                              cards.RemoveAt(i);
                              cards.Insert(i, card_object);
                              card_object.GetComponent<Card>().card_number = i;

                              // 터치영역 세팅
                              touchAreas[i].GetComponent<CardTouch>().card = card_object.GetComponent<Card>();
                              touchAreas[i].SetActive(true);

                              ShuffleChance(i);

                              card_enable[i] = true;
                              return;
                        }
                  }
            }
      }*/

      /* public IEnumerator Sorting2(int card_number)
       {

             PanelsOn();

             // new code


             int false_count = 0;
             for (int i = 0; i < 8; i++)
             {
                   if (card_enable[i] == false)
                   {
                         false_count++;
                   }
             }

             // new code
             //cards.RemoveAt(card_number);
             Destroy(cards[card_number]);


             if (false_count % 2 == 1)
             {


                   // 뒤 제거 : 뒤 카드를 앞으로 땡김
                   // 3을 지우면?

                   for (int i = card_number; i < cards.Count - 1; i++)
                   {
                         //   Debug.Log("뒤제거 i : " + i);

                         if (card_enable[i + 1] != false)
                         {
                               Debug.Log((i + 1) + " 를 " + i + "로 이동");
                               cards[i + 1].GetComponent<Card>().StartCoroutine("CardSorting", card_transfomrs[i]);
                         }
                         yield return new WaitForSeconds(0.09f);



                         if (card_enable[i + 1] == false)
                         {
                               card_enable[i] = false;
                               break;
                         }


                   }
                   for (int i = card_number + 1; i < cards.Count; i++)
                   {
                         if (cards[i] != null)
                         {
                               cards[i].GetComponent<Card>().card_number = i - 1;
                               //      touchAreas[i].GetComponent<CardTouch>().card = cards[i].GetComponent<Card>();
                         }
                         cards[i - 1] = cards[i];


                   }
                   cards[7] = null;
                   // touchAreas[0].GetComponent<CardTouch>().card = null;


                   //          cards[7].SetActive(false);
                   card_enable[7] = false;
             }
             else
             {
                   // 앞 제거 : 앞 카드를 뒤로 밀기
                   for (int i = card_number; i > 0; i--)
                   {
                         //   Debug.Log("앞제거 i : " + i);

                         if (card_enable[i - 1] != false)
                         {
                               cards[i - 1].GetComponent<Card>().StartCoroutine("CardSorting", card_transfomrs[i]);
                         }

                         yield return new WaitForSeconds(0.09f);
                         if (card_enable[i - 1] == false)
                         {
                               card_enable[i] = false;
                               break;
                         }
                   }

                   for (int i = card_number - 1; i >= 0; i--)
                   {
                         if (cards[i] != null)
                         {
                               cards[i].GetComponent<Card>().card_number = i + 1;
                               //  touchAreas[i].GetComponent<CardTouch>().card = cards[i].GetComponent<Card>();
                         }
                         cards[i + 1] = cards[i];
                   }
                   cards[0] = null;
                   // touchAreas[0].GetComponent<CardTouch>().card = null;
                   card_enable[0] = false;

             }

             for (int i = 0; i < cards.Count; i++)
             {
                   if (cards[i] == null)
                   {
                         touchAreas[i].GetComponent<CardTouch>().card = null;
                         touchAreas[i].SetActive(false);
                   }
                   else
                   {
                         cards[i].GetComponent<Card>().card_number = i;
                         touchAreas[i].GetComponent<CardTouch>().card = cards[i].GetComponent<Card>();
                   }
             }

             PanelsOff();
             yield return null;
       }*/

      /*public void ViewCardInfo(GameObject magic_prefab)
      {
            CardView cardView = card_view_object.GetComponent<CardView>();
            cardView.SetCardInfo(magic_prefab);
            card_view_object.SetActive(true);
      }*/
      /* public void OffCardInfo()
       {
             card_view_object.SetActive(false);
       }*/
      /*public IEnumerator AutoCardAdd()
      {
            current_time = 0;
            cardSlider.value = current_time;
            while (true)
            {

                  while (current_time < data._turnPeriod)
                  {
                        yield return new WaitForSeconds(0.1f);
                        current_time += 0.1f;
                        cardSlider.value = current_time;
                        //draw_text.text = string.Format("{0:F1}",current_time);
                  }
                  yield return new WaitForSeconds(0.3f);
                  if (_sorting == false)
                  {
                        Debug.Log("Card Add");
                        if (pettyGreed != null)
                        {
                              pettyGreed.current_count++;
                        }
                        StartCoroutine("AddCard");

                        current_time = 0;
                        cardSlider.value = current_time;
                        //addFlag = false;
                  }
            }
      }*/

      /*public void CheckAllEdge()
      {
            for (int i = 0; i < cards.Count; i++)
            {
                  if (cards[i] != null)
                  {
                        cards[i].GetComponent<Card>().CheckEdge();
                  }
            }
      }
      public void ActiveAllEdge()
      {
            for (int i = 0; i < cards.Count; i++)
            {
                  if (cards[i] != null)
                  {
                        // cards[i].GetComponent<Card>().StartCoroutine("EdgeEffect");
                  }
            }
      }
      */

      // Add Card
      /*Debug.Log("AddCard start");
           // 앞에서부터 추가.
           while (_sorting == true || _adding == true)
           {
                 //sorting 진행중이면.
                 yield return new WaitForSeconds(0.05f);
           }
           _adding = true;


           Debug.Log("AddCard Coroutine");
           bool flag = false;
           for (int i = 9; i >= 0; i--)
           {
                 if (card_enable[i] == false)
                 {
                       Debug.Log("CardAdded");
                       // new code
                       GameObject card_object = Instantiate(card_prefab, card_transfomrs[i], false);
                       card_object.SetActive(false);
                       StartCoroutine(ParticleCard(i, card_object));

                       card_object.GetComponent<Card>().transform.localPosition = new Vector2(0, 0);
                       card_object.GetComponent<Card>().card_number = i;
                       cards.RemoveAt(i);
                       cards.Insert(i, card_object);

                       // 터치영역 세팅
                       touchAreas[i].GetComponent<CardTouch>().card = card_object.GetComponent<Card>();
                       touchAreas[i].SetActive(true);

                       ShuffleChance(i);

                       flag = true;
                       card_enable[i] = true;
                       //cards[i].SetActive(true);
                       break;
                 }
           }
           //sortFlag = false;
           if (flag == false)
           {
                 _adding = false;
           }

           Debug.Log("AddCard End");
           yield return null;*/

}
