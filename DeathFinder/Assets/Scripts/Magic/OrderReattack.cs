using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderReattack : MagicBase
{
    void Start()
    {
            GameController.Instance.OrderReattack();
            Destroy(gameObject);
      }
    
}
