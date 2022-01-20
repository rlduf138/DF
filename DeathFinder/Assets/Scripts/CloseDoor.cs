using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseDoor : MonoBehaviour
{
      public Sprite[] anim_sprites;
      public Image animImage;
      public GameObject leftDoor;
      public GameObject rightDoor;

      // Start is called before the first frame update
      void Start()
      {
            StartCoroutine("DoorMove");
      }

      // Update is called once per frame
      void Update()
      {

      }

      public IEnumerator DoorMove()
      {
            float currentTime = 0f;

            Vector3 leftDirection = Vector3.zero - leftDoor.transform.position;
            leftDirection.Normalize();
            Vector3 rightDirection = Vector3.zero - rightDoor.transform.position;
            rightDirection.Normalize();

            while (currentTime < 0.6f)
            {
                  yield return new WaitForFixedUpdate();

                  if (leftDoor.transform.position.x < 0f)
                  {
                        leftDoor.transform.transform.Translate(leftDirection * 77f * Time.deltaTime);
                  }
                  if (rightDoor.transform.position.x > 0f)
                  {
                        rightDoor.transform.transform.Translate(rightDirection * 77f * Time.deltaTime);
                  }
                  currentTime += Time.deltaTime;
            }
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);
            StartCoroutine("AnimationDoor");
      }
      public IEnumerator AnimationDoor()
      {
            animImage.gameObject.SetActive(true);

           for(int i =0; i<anim_sprites.Length; i++)
            {
                  animImage.sprite = anim_sprites[i];
                  yield return new WaitForSeconds(0.05f);
            }
            
      }
}
