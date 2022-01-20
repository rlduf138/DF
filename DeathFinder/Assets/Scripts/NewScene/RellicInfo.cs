using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelSilo.Helper;

public class RellicInfo : Singleton<RellicInfo>
{
      public TextMeshProUGUI rellicNameText;
      public TextMeshProUGUI rellicExplainText;

      public GameObject infoBox;

      public Image rellicPoint;
      private Image mask;

      
      void Start()
      {
            mask = infoBox.GetComponent<Image>();
      }

      void Update()
      {

      }

      public void RellicClick()
      {
            StartCoroutine(RellicPointer());
      }

      public void ButtonClick()
      {
            // 설명창 확인 버튼 클릭.
            rellicPoint.fillAmount = 0;
            mask.fillAmount = 0;
            infoBox.SetActive(false);

            // 카트 선택 창으로.
            //cardCanvas.SetActive(true);  gassi_front 애니메이션 이벤트로 
            SeaController.Instance.OnGgasi02();

      }

      public IEnumerator RellicPointer()
      {
            Debug.Log("RellicPointer Coroutine");
            // 유물 화살표 슬라이더
            while(rellicPoint.fillAmount < 1)
            {
                  rellicPoint.fillAmount += 0.05f;

                  yield return new WaitForFixedUpdate();
            }

            Debug.Log("RellicPointer Coroutine End");

            // 유물 설명창 On
            StartCoroutine(InfoBoxOn());
      }

      public IEnumerator InfoBoxOn()
      {
            Debug.Log("InfoBoxOn Coroutine");

            infoBox.SetActive(true);
            CheckSize();

            while (mask.fillAmount < 1)
            {
                  mask.fillAmount += 0.05f;

                  yield return new WaitForFixedUpdate();
            }
            Debug.Log("InfoBoxOn Coroutine End");
      }

      public void CheckSize()
      {
            Debug.Log("CheckSize()");
            //  rellicExplainText.
            RectTransform rectTrans = infoBox.GetComponent<RectTransform>();
            RectTransform textTrans = rellicExplainText.GetComponent<RectTransform>();
            // height + 19.12
            if(textTrans.rect.height <= 80f)
            {
                  Debug.Log("min Size");
                  rectTrans.sizeDelta = new Vector2(rectTrans.rect.width, 150f);
                  //rectTrans.rect.Set(rectTrans.rect.x, rectTrans.rect.y, rectTrans.rect.width, 150f);
                
            }
            else
            {
                  Debug.Log("add Size");
                  rectTrans.sizeDelta = new Vector2(rectTrans.rect.width, textTrans.rect.height + 100f);
                  //rectTrans.rect.Set(rectTrans.rect.x, rectTrans.rect.y, rectTrans.rect.width, textTrans.rect.height+70f);
            }
            
      }
}
