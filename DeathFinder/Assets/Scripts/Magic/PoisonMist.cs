using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMist : MagicBase
{
      public float poisonDamage;
      public float poisonTime;
    // Start is called before the first frame update
    void Start()
    {
            GameController.Instance.PoisonCard(poisonDamage, poisonTime);
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
