using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      public void OnMouseUp()
      {
            Debug.Log("Click");
            if (GetComponent<Image>().color.a >= 0.7f)
            {
                  GameController.Instance.OnRestartClick();
            }
      }
}
