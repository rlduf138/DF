using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderAssemble : MagicBase
{


      void Start()
      {
            GameController.Instance.OrderAssemble();
            Destroy(gameObject);
      }
}
