using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMoveOff : MagicBase
{
    // Start is called before the first frame update
    void Start()
    {
            GameController.Instance.OrderMoveOff();
            Destroy(gameObject);
      }

}
