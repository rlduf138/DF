using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMoving : MonoBehaviour
{
      public float driftTime = 0f;
      Vector3 _driftNextPosition;
      public Transform parent_trans;

      private void FixedUpdate()
      {

            Vector3 direction = _driftNextPosition - transform.position;
            
            
            if (driftTime < 2f)
            {
                  parent_trans.transform.Translate(direction * 0.1f * Time.deltaTime);
                  _driftNextPosition = new Vector2(transform.position.x, transform.position.y + 1);
                  driftTime += Time.deltaTime;
            }
            else if (driftTime < 4f)
            {
                  parent_trans.transform.Translate(direction * 0.1f * Time.deltaTime);
                  _driftNextPosition = new Vector2(transform.position.x, transform.position.y - 1);
                  driftTime += Time.deltaTime;
            }
            else
            {
                  driftTime = 0f;
            }

      }
}
