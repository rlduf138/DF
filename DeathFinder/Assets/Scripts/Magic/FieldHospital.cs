using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldHospital : MagicBase
{
      public float heal;
      public float duration;
      public float time;
      private float current_time;
      private float current_duration;
      private bool canHeal;

      private Vector2 origin;
      private Vector2 size;
      public LayerMask layerMask;

      // Start is called before the first frame update
      void Start()
      {
            Debug.Log("FieldHospial On");
            transform.position = GameController.Instance.group.transform.position;
            StartCoroutine("Heal");
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            origin = transform.position;
            size = collider.size;
      }

      // Update is called once per frame
      void Update()
      {
            if (time <= current_time)
            {
                  Destroy(gameObject);
            }
            current_time += Time.deltaTime;
      }
      public IEnumerator Heal()
      {
            while (true)
            {
                  if (current_duration >= duration)
                  {
                        // 범위 힐.
                        canHeal = true;
                        AreaHeal();
                        current_duration = 0;
                  }
                  yield return new WaitForSeconds(0.1f);
                  current_duration += 0.1f;
            }
      }
      private void AreaHeal()
      {
            Debug.Log("AreaHeal()");
            RaycastHit2D[] rh2ds = Physics2D.BoxCastAll(origin, size, 0f, transform.up, 1f, layerMask);
            if(rh2ds != null)
            {
                  for(int i = 0; i< rh2ds.Length; i++)
                  {
                        rh2ds[i].collider.GetComponent<LivingEntity>().RestoreHealth(heal);
                        Debug.Log("AreaHeal i : " + rh2ds[i].collider.GetComponent<MinionBase>().index + " - restorehealth");
                  }
            }
            canHeal = false;
      }
      private void OnDrawGizmosSelected()
      {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(origin, size);
            
      }
}
