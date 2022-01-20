using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScan : MonoBehaviour
{
      DemonBase demonBase;
      private CapsuleCollider2D coll;

      [Header("ScanRange")]
      public float sc_xRadius;
      public float sc_yRadius;
      public float offset_x;
      public float offset_y;
      private Vector2 size;
      private Vector2 offset;

      public void InitDemonScan(DemonBase db, float x, float y, float off_x, float off_y)
      {
            demonBase = db;
            sc_xRadius = x;
            sc_yRadius = y;
            offset_x = off_y;
            offset_y = off_y;
            size = new Vector2(sc_xRadius, sc_yRadius);
            offset = new Vector2(0, offset_y);
            if (db.gameObject.tag == "Player")
            {
                  tag = "Player";
            }else if(db.gameObject.tag == "Player2")
            {
                  tag = "Player2";
            }
            coll = GetComponent<CapsuleCollider2D>();
            coll.size = size;
            coll.offset = offset;
      }
      // Start is called before the first frame update
      void Start()
      {
            
      }
      
      private void OnTriggerStay2D(Collider2D collision)
      {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player2")
                  && tag == "Player")
            {
                 // Debug.Log("감지P1" + demonBase.gameObject.name);
                  demonBase.first_pos_flag = true;
                  demonBase._nextPosition = collision.transform.localPosition;
                  demonBase.scan_flag = true;
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Player")
                  && tag == "Player2")
            {
                //  Debug.Log("감지P2" + demonBase.gameObject.name);
                  demonBase.first_pos_flag = true;
                  demonBase._nextPosition = collision.transform.localPosition;
                  demonBase.scan_flag = true;
            }
      }
      private void OnTriggerExit2D(Collider2D collision)
      {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player2")
                  && tag == "Player")
            {
                  Debug.Log("감지 대상 나감 P1" + demonBase.gameObject.name);
                  
                  demonBase.scan_flag = false;
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Player")
                  && tag == "Player2")
            {
                  Debug.Log("감지 대상 나감 P2" + demonBase.gameObject.name);
                  
                  demonBase.scan_flag = false;
            }
      }
}
