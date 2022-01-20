using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCharge : MagicBase
{

    // Start is called before the first frame update
    void Start()
    {
            GameController.Instance.OrderCharge();
            Destroy(gameObject);
      }

    
}
