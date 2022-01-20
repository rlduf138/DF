using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartingCard : MonoBehaviour
{
      public int card_number;

      public int magic_index;
      public GameObject edge;
      public Image portrait;
      public TextMeshProUGUI cost_text;
      public TextMeshProUGUI name_text;
      public TextMeshProUGUI explain_text;

      public bool select;

      public GameObject magicPrefab;

      // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {

      }

      public void SetMagic(GameObject magic)
      {
            
            magicPrefab = magic;
            MagicBase mb = magicPrefab.GetComponent<MagicBase>();

            magic_index = mb.macigIndex;
            Init(mb.magicSprite, mb.cost, mb.magicName, mb.magicExplain);
      }

      public void Init(Sprite sprite, int cost, string name, string explain)
      {
            portrait.sprite = sprite;
            cost_text.text = cost.ToString();
            name_text.text = name;
            explain_text.text = explain;
      }

      public void OnClick()
      {
            // 카드 터치되면.
           
            if(select == true)
            {
                  Debug.Log(card_number + " 번 카드 선택해제");
                  select = false;
                  edge.SetActive(false);
                  // controller에 전송
                  StartingCardController.Instance.ReleaseCard(card_number);
            }else if(select == false)
            {
                  Debug.Log(card_number + " 번 카드 선택");
                  select = true;
                  edge.SetActive(true);
                  // controller에 전송
                  StartingCardController.Instance.SelectCard(card_number);
            }
      }

      public void EdgeOff()
      {
            edge.SetActive(false);
      }
}
