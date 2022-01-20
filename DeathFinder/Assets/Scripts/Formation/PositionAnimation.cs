using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionAnimation : MonoBehaviour
{
      public Sprite[] arrowSprite;
      public Sprite[] edgeSprite;

      public Image arrowImage;
      public Image edgeImage;

      void Start()
      {
            arrowImage = GetComponent<Image>();
            edgeImage = GetComponentsInChildren<Image>()[1];
            StartCoroutine("ArrowAnimation");
            StartCoroutine("EdgeAnimation");
      }

      public IEnumerator ArrowAnimation()
      {
            int i = 0;
            while (true)
            {
                  if (i == arrowSprite.Length) i = 0;

                  arrowImage.sprite = arrowSprite[i];
                  yield return new WaitForSeconds(0.1f);
                  i++;
            }
      }
      public IEnumerator EdgeAnimation()
      {
            int i = 0;
            while (true)
            {
                  if (i == edgeSprite.Length) i = 0;

                  edgeImage.sprite = edgeSprite[i];
                  yield return new WaitForSeconds(0.1f);
                  i++;
            }
      }
}
