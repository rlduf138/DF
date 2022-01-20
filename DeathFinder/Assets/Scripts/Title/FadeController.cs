using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
      public Image fadeImg;

      public void FadeIn(float fadeInTime, System.Action nextEvent = null)
      {
            StartCoroutine(CoFadeIn(fadeInTime, nextEvent));
      }
      public void FadeOut(float fadeOutTime, System.Action nextEvent = null)
      {
            StartCoroutine(CoFadeOut(fadeOutTime, nextEvent));
      }

      public void InitColor(Color color)
      {
            fadeImg.color = color;
      }

      IEnumerator CoFadeIn(float fadeInTime, System.Action nextEvent = null)
      {
            //SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
            Image sr = fadeImg;
            Color tempColor = sr.color;
            while (tempColor.a < 1f)
            {
                  tempColor.a += Time.deltaTime / fadeInTime;
                  sr.color = tempColor;

                  if (tempColor.a >= 1f) tempColor.a = 1f;

                  yield return null;
            }

            sr.color = tempColor;
            if (nextEvent != null) nextEvent();
      }

      IEnumerator CoFadeOut(float fadeOutTime, System.Action nextEvent = null)
      {
            Image sr = fadeImg;
            Color tempColor = sr.color;
            while (tempColor.a > 0f)
            {
                  tempColor.a -= Time.deltaTime / fadeOutTime;
                  sr.color = tempColor;

                  if (tempColor.a <= 0f) tempColor.a = 0f;

                  yield return null;
            }

            sr.color = tempColor;
            if (nextEvent != null) nextEvent();
      }

      // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {

      }
}
