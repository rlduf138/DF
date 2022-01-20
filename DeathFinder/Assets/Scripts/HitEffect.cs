using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
      public int effectCount;
      private Animator animator;

      // Start is called before the first frame update
      void Start()
      {
            animator = GetComponent<Animator>();

            int ran = Random.Range(0, effectCount);
            Debug.Log("ran : " + ran);
            animator.SetInteger("random",ran);
      }

     
}
