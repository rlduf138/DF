using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulliganController : Singleton<MulliganController>
{
      public List<GameObject> mulligan_selected;

      public Card[] cards;

      public GameObject alert_panel;

      [Header("Selected_0_Tier")]
      public List<GameObject> tier_0_list;

      [Header("Selected_1_Tier")]
      public List<GameObject> tier_1_list;

      public void RefreshMulligan()
      {
            //mulligan_selected = null;
            
                  mulligan_selected.RemoveRange(0, mulligan_selected.Count);
            

            for(int i = 0; i<8; i++)
            {
                  if(cards[i].mulligan_select == true)
                  {
                        mulligan_selected.Add(cards[i].magic_prefab);
                  }
            }
            if(mulligan_selected.Count == 3)
            {
                  // 3개 선택완료. 확정 얼럿
                  alert_panel.SetActive(true);
            }
      }
      public void AgreeClick()
      {
            alert_panel.SetActive(false);
            gameObject.SetActive(false);

            CardController.Instance.except_cards = mulligan_selected;

        //    CardController.Instance.tier_0_list = tier_0_list;
          //  CardController.Instance.tier_1_list = tier_1_list;

            GameController.Instance.GameStart();
      }
      public void CancelClick()
      {
            alert_panel.SetActive(false);
            for(int i = 0; i<8; i++)
            {
                  cards[i].InitMulligan();
            }
      }

      public void MulliganSetting()
      {
            int count = 0;

            for(int i = 0; i < tier_0_list.Count; i++)
            {
                  Debug.Log("Card[" + i + "] : " + tier_0_list[i]);
                  cards[i].SetMagic(tier_0_list[i]);
                  count++;
                  //5개 였다면. count = 5
            }
            for(int i = 0; i < 8-tier_0_list.Count; i++)
            {
                  
                  Debug.Log("Card[" + (i + tier_0_list.Count) + "] : " + tier_1_list[i]);
                  cards[i+tier_0_list.Count].SetMagic(tier_1_list[i]);
                  // i + 5
                  count++;
            }
      }

}
