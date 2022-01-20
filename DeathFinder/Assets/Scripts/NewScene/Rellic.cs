using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rellic : MonoBehaviour
{
      private Rigidbody2D rgbd;
      private Transform trans;
      int moveFlag = 0;

      // Start is called before the first frame update
      void Start()
      {
            rgbd = GetComponent<Rigidbody2D>();
            trans = GetComponent<Transform>();
      }

      // Update is called once per frame
      void Update()
      {

      }

      private void FixedUpdate()
      {
            if (moveFlag < 40)
            {
                  trans.position = new Vector2(trans.position.x, trans.position.y + 0.01f);
                  moveFlag++;
            }else if(moveFlag < 80)
            {
                  trans.position = new Vector2(trans.position.x, trans.position.y - 0.01f);
                  moveFlag++;
            }else if(moveFlag == 80)
            {
                  moveFlag = 0;
            }

      }
      public void RellicClick()
      {
            Debug.Log("Rellick Click()");

            RellicInfo.Instance.RellicClick();
      }
}
