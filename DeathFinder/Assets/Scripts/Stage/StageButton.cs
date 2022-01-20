using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
      public GameObject selectGroup;
      public int stageNumber;

      public float bossHp;
      public float bossDamage;
      public int enemy_number;

      // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {

      }

      public void OnMouseUp()
      {
            // 카메라 이동중이면 작동 안함.
            if (StageController.Instance.cameraMoveFlag == false)
            {
                  StartCoroutine("CameraMove");
                  StageController.Instance.ChangeSelectedStage(this);
            }
      }
      public IEnumerator CameraMove()
      {
            // 스테이지 클릭시 카메라를 이동
            StageController.Instance.startButton.enabled = false;
            StageController.Instance.cameraMoveFlag = true;
            StageController.Instance.AllSelectGroupOff();

            Vector3 _nextPosition = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
            Vector3 direction = _nextPosition - Camera.main.transform.position;
            direction.Normalize();

            if (direction.x >= 0)
            {
                  while (Camera.main.transform.position.x < transform.position.x)
                  {
                        yield return new WaitForFixedUpdate();

                        Camera.main.transform.transform.Translate(direction * 30f * Time.deltaTime);
                  }
            }
            else if (direction.x < 0)
            {
                  while (Camera.main.transform.position.x > transform.position.x)
                  {
                        yield return new WaitForFixedUpdate();

                        Camera.main.transform.transform.Translate(direction * 30f * Time.deltaTime);
                  }
            }
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

            selectGroup.SetActive(true);

            StageController.Instance.cameraMoveFlag = false;
            StageController.Instance.startButton.enabled = true;

            // 버튼 생성, 하단 카드 생성..
      }
      public void CameraTeleport()
      {
            selectGroup.SetActive(true);
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
      }
}
