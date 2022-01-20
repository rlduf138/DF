using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaEater : MagicBase
{
      public int minMana;
      public int maxMana;

      void Start()
      {
            GameController.Instance.ManaEater(minMana, maxMana);
            Destroy(gameObject);
      }

      void Update()
      {

      }
}
