using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MagicBase
{
      public float duration_time;
      public float current_time = 0f;
      void Start()
      {
            GameController.Instance.BarrierCard();
      }

      void Update()
      {
            if (current_time < duration_time)
            {
                  current_time += Time.deltaTime;
            }
            else
            {
                  Debug.Log("Barrier Destroy");
                  GameController.Instance.BarrierCardEnd();
                  Destroy(gameObject);
            }
      }
}
