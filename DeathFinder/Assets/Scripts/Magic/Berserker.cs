using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : MagicBase
{
      public float duration_time;
      public float current_time = 0f;

      // Start is called before the first frame update
      void Start()
      {
            GameController.Instance.BerserkerCard();
      }

      // Update is called once per frame
      void Update()
      {
            if(current_time < duration_time)
            {
                  current_time += Time.deltaTime;
            }
            else
            {
                  Debug.Log("Berserker Destroy");
                  GameController.Instance.BerserkerCardEnd();
                  Destroy(gameObject);
            }
      }
}
