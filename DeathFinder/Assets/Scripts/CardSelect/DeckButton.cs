using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{
      public GameObject cardObject;
      public Card_CS card_cs;

      public TextMeshProUGUI nameText;
      public TextMeshProUGUI costText;

      private void Start()
      {
            card_cs = cardObject.GetComponent<Card_CS>();
            nameText.text = card_cs.card_name;
            costText.text = card_cs.cost.ToString();
      }

      public void OnClick()
      {
            CardSelectController.Instance.DeleteDeck(cardObject);
      }
}
