using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : MagicBase
{
      public float percent;

      void Start()
      {
            GameController.Instance.Sacrifice(percent);
            Destroy(gameObject);
      }

      // Update is called once per frame
      void Update()
      {

      }
}
