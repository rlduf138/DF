using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revival : MagicBase
{
    // Start is called before the first frame update
    void Start()
    {
            Debug.Log("Revival");
            GameController.Instance.StartCoroutine("RevivalCard");
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
