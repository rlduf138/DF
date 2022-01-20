using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MagicBase
{

      public float percent;
      public float duration_time;
      public float current_time = 0f;


      void Start()
      {
            GameController.Instance.FreezeCardOn(percent);
      }

      void Update()
      {
            if (current_time < duration_time)
            {
                  current_time += Time.deltaTime;
            }
            else
            {
                  Debug.Log("Curse Destroy");
                  GameController.Instance.FreezeCardEnd();
                  Destroy(gameObject);
            }
      }
}
