using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBoom : MagicBase
{
      public int minDamage;
      public int maxDamage;

    void Start()
    {
            GameController.Instance.ManaBoomCard(minDamage, maxDamage);
            Destroy(gameObject);
      }

    void Update()
    {
        
    }
}
