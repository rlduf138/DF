using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PettyGreed : MagicBase
{
      public int count;
      public int current_count = 0;
      public float percent;

      // Start is called before the first frame update
      void Start()
      {
            CardController.Instance.PettyGreed(count, percent);
      }

      // Update is called once per frame
      void Update()
      {
            if(count == current_count)
            {
                  CardController.Instance.PettyGreedEnd();
                  Destroy(gameObject);
            }
      }
}
