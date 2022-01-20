using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircle : MonoBehaviour
{
      public float shake_time;
      public float shake_range;

      public void Shake()
      {
            CameraScript.Instance.Shake(shake_time, shake_range);
      }
      public void Spawn()
      {
            GameController.Instance.StartCoroutine("SpawnWithFormation");
      }
}
