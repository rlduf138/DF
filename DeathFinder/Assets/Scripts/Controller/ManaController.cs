using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaController : Singleton<ManaController>
{
      public BattleData data;
      public Slider mana_slider;
      public Image background_img;

     // public Sprite[] manaBarSprites;
      public Image manaAnimation;

      public TextMeshProUGUI manaText;

      [Header("mana option")]
      public int max_mana;
    
      private float current_time;

      // Start is called before the first frame update
      void Start()
      {
            max_mana = data._maxMana + data._maxManaLevel;
            mana_slider.maxValue = max_mana;
            data._currentMana = data._manaStart;
      }

      public void Starting()
      {
            ManaBarRefresh();
            //StartCoroutine("ManaBarAnimation");
      }
      
      public IEnumerator ManaRestore()
      {
            while (true)
            {
                  current_time = data._turnPeriod;
                  
                  while (current_time > 0)
                  {
                        yield return new WaitForSeconds(0.5f);
                        current_time -= 0.5f;
                  }
                  RestoreMana(data._manaRestore + data._manaRestoreLevel);
                  Debug.Log("ManaRestore");
            }
      }

     /* public IEnumerator ManaBarAnimation()
      {
            int i = 0;

            while (true)
            {
                  manaAnimation.sprite = manaBarSprites[i];
                  i++;
                  yield return new WaitForSeconds(0.3f);
                  if(i == manaBarSprites.Length)
                  {
                        i = 0;
                  }
            }
      }*/

      public void RestoreMana(int addmana)
      {
            Debug.Log("RestoreMana : " + addmana);
            if (data._currentMana + addmana >= max_mana)
            {
                  data._currentMana = max_mana;
            }
            else
            {
                  data._currentMana += addmana;
            }
            ManaBarRefresh();
           
      }

      public bool UseMana(int useMana)
      {
            if(useMana > data._currentMana)
            {
                  // 마나가 부족하면 false.
               //   StartCoroutine("ManaAlert");
                  return false;
            }

            
            data._currentMana -= useMana;
            ManaBarRefresh();
            return true;
      }

    /*  public IEnumerator ManaAlert()
      {
            background_img.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            background_img.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            background_img.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            background_img.color = Color.white;
            
      }*/

      protected void ManaBarRefresh()
      {
            // -22 ~ 25  0.021
            mana_slider.value = data._currentMana;
         //   CardController.Instance.CheckAllEdge();
            manaText.text = data._currentMana.ToString() + "/" + max_mana.ToString();
      }

}
