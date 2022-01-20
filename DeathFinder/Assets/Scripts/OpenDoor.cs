using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
      public Sprite[] anim_sprites;
      public Image animImage;
      public GameObject leftDoor;
      public GameObject rightDoor;

      // Start is called before the first frame update
      void Start()
      {
            StartCoroutine("AnimationDoor");
      }

      // Update is called once per frame
      void Update()
      {

      }

      public IEnumerator DoorMove()
      {
            float currentTime = 0f;

            leftDoor.SetActive(true);
            rightDoor.SetActive(true);



            Vector3 leftDirection = new Vector3(-250f, 0f) - leftDoor.transform.position;
            leftDirection.Normalize();
            Vector3 rightDirection = new Vector3(250f, 0f) - rightDoor.transform.position;
            rightDirection.Normalize();

            while (currentTime < 0.6f)
            {
                  yield return new WaitForFixedUpdate();

                  leftDoor.transform.transform.Translate(leftDirection * 77f * Time.deltaTime);
                  rightDoor.transform.transform.Translate(rightDirection * 77f * Time.deltaTime);
                  currentTime += Time.deltaTime;
            }
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);

            GameController.Instance.GameStart();
            gameObject.SetActive(false);
      }
      public IEnumerator AnimationDoor()
      {
            animImage.gameObject.SetActive(true);
            for (int i = 0; i < anim_sprites.Length; i++)
            {
                  animImage.sprite = anim_sprites[i];
                  yield return new WaitForSeconds(0.05f);
            }
            StartCoroutine("DoorMove");
            animImage.gameObject.SetActive(false);

      }
}
