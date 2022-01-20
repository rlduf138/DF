using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_CS : MonoBehaviour
{
      [Header("Value")]
      public Sprite card_sprite;
      public string card_name;
      public string card_explain;
      public int index;
      public int cost;
      public int type;

      [Header("UI")]
      public GameObject edgeImg;
      public Image cardImage;
      //public Text nameText;
      // public Text explainText;
      public TextMeshProUGUI nameText;
      public TextMeshProUGUI explainText;
      public Text costText;

      [Header("ETC")]
      public bool IsDeck;
      public GameObject deckObject;

      void Start()
      {
            Init();
            
      }

      public void Init()
      {
            cardImage.sprite = card_sprite;
           // cardImage.SetNativeSize();
            nameText.text = card_name;
            explainText.text = card_explain;
            costText.text = cost.ToString();
            RefreshEdge();
      }
      public void RefreshEdge()
      {
            if(IsDeck == true)
            {
                  edgeImg.SetActive(true);
            }else if(IsDeck == false)
            {
                  edgeImg.SetActive(false);
            }
      }

      public void OnClick()
      {
            if (IsDeck == false)
            {
                  Debug.Log("AddDeck");
                  CardSelectController.Instance.AddDeck(gameObject);
            }else if(IsDeck == true)
            {
                  Debug.Log("DeleteDeck");
                  CardSelectController.Instance.DeleteDeck(gameObject);
            }
            RefreshEdge();
      }

}
