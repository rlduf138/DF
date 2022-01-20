using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
      public GameObject magicObject;
      public Image magic_image;
      public TextMeshProUGUI name_text;
      public TextMeshProUGUI explain_text;
      public TextMeshProUGUI cost_text;

      public void SetCardInfo(GameObject magic_prefab)
      {
            magicObject = magic_prefab;
            magic_image.sprite = magic_prefab.GetComponent<MagicBase>().magicSprite;
            magic_image.SetNativeSize();
            name_text.text = magic_prefab.GetComponent<MagicBase>().magicName;
            explain_text.text = magic_prefab.GetComponent<MagicBase>().magicExplain;


            cost_text.text = magic_prefab.GetComponent<MagicBase>().cost.ToString();
      }
}
