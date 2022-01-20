using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GgasiAnimation : MonoBehaviour
{
    
      public void RellicOn()
      {
            SeaController.Instance.RellicOn();
      }
      public void CardOn()
      {
            SeaController.Instance.CardCanvasOn();
      }
}
