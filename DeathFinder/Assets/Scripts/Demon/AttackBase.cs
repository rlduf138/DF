using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
      [Header("Layermask")]
      public LayerMask mainLayerMask;
      public LayerMask p1_layerMask;
      public LayerMask p2_layerMask;

      private CapsuleCollider2D coll;
      protected DemonBase demonBase;

      protected float damage;
      protected float attack_speed;
      protected float xRadius;
      protected float yRadius;

      protected int attack_max_count;

      protected float offset_x;
      protected float offset_y;

      protected Vector2 offset;
      public Vector2 origin;
      public Vector2 size;

      protected float translate_atspd;      // 환산 공격속도 = 1 / attack_speed

      protected float lastAttackTime = 0f;



      // 문서 상 표기 : 1초에 n번 공격.
      // 1초에 x번으로 환산 :  1/n

      public void InitMeleeAttack(DemonBase db, float dmg, float aspd, int max_count, float xRad, float yRad, float off_x, float off_y)
      {
            demonBase = db;
            damage = dmg;
            attack_speed = aspd;
            attack_max_count = max_count;

            xRadius = xRad;
            yRadius = yRad;
            size = new Vector2(xRadius, yRadius);

            translate_atspd = 1 / attack_speed;
            Debug.Log("translate_atspd : " + translate_atspd);

            offset_x = off_x;
            offset_y = off_y;
            offset = new Vector2(offset_x, offset_y);

            coll = GetComponent<CapsuleCollider2D>();
            coll.size = size;
            coll.offset = offset;
      }
      public void ChangeDamage(float dmg)
      {
            damage += dmg;
      }
      public void ChangeAttackSpeed(float asp)
      {
            attack_speed += asp;
      }
     

      // Start is called before the first frame update
      void Start()
    {
            if (demonBase.tag == "Player")
            {
                  tag = "Player";
                  SetP1LayerMask();
            }
            else if (demonBase.tag == "Player2")
            {
                  tag = "Player2";
                  SetP2LayerMask();
            }
      }

    // Update is called once per frame
    void Update()
      {
            if (demonBase.attack == true)
            {
                  if (lastAttackTime > translate_atspd)
                  {
                        demonBase.attack = false;
                        lastAttackTime = 0;

                        StartCoroutine("AttackGap");
                  }
                  lastAttackTime += Time.deltaTime;
            }

      }
      public IEnumerator AttackGap()
      {
            float speed = demonBase.cur_speed;
            demonBase.cur_speed = 0f;

            yield return new WaitForSeconds(0.02f);

            demonBase.cur_speed = speed;
      }
      protected RaycastHit2D[] Sorting(RaycastHit2D[] rh2ds)
      {
            // 공격 거리 안에있는 몬스터 거리에 따라서 정렬.
            RaycastHit2D temp;

            int i, j;

            for (i = 0; i < rh2ds.Length - 1; i++)
            {

                  j = i;
                  while (j >= 0 && (rh2ds[j].transform.position - transform.position).sqrMagnitude > (rh2ds[j + 1].transform.position - transform.position).sqrMagnitude)
                  {
                        temp = rh2ds[j];
                        rh2ds[j] = rh2ds[j + 1];
                        rh2ds[j + 1] = temp;

                        j--;
                  }
            }


            return rh2ds;
      }

      protected virtual void Attack()
      {
            if (demonBase.Dead == true)
            {
                  return;
            }
      }


      public void SetP1LayerMask()
      {
            mainLayerMask = p1_layerMask;
      }
      public void SetP2LayerMask()
      {
            mainLayerMask = p2_layerMask;
      }
      private void OnDrawGizmosSelected()
      {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(origin, size);
      }
}
