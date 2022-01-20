using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCard : MagicBase
{

    void Start()
    {
            GameController.Instance.StartCoroutine("DoubleCard");
            Destroy(gameObject);
      }

    void Update()
    {
        
    }
}
