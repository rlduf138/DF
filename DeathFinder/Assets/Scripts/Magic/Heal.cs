using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MagicBase
{
      public float percent;

      void Start()
      {
            GameController.Instance.HealCard(percent);
            Destroy(gameObject);
      }

      void Update()
      {

      }
}
