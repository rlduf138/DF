using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DemonButton : MonoBehaviour
{
      public FormationController formationController;
      public GameObject select;
      Material material;
      public Canvas canvas;

      private Image demonImage;
      public int demon_index;
      public string _name;
      public Sprite demonSprite;
      public int attackLevel;
      public int healthLevel;
      public int rangeLevel;

      public Sprite[] animation_sprites;


      // Start is called before the first frame update
      void Start()
      {
            formationController = FindObjectOfType<FormationController>();
            demonImage = GetComponent<Image>();
          
            demonImage.SetNativeSize();
            //StartCoroutine("Animation");
        
      }

      

      public void ButtonClick()
      {
            select.SetActive(true);
            formationController.SettingPositionButton(this);
            formationController.DemonCardClick(_name, demonSprite, attackLevel, healthLevel, rangeLevel, animation_sprites, this);
      }

   
}
