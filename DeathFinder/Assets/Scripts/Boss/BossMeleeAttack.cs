using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
      private CapsuleCollider2D coll;
      public BossBase bossBase;
      public GameObject bossSkill;

      public float damage;
      public float attack_speed;

      public float skillDamage;

      protected int attack_max_count;

      protected float offset_x;
      protected float offset_y;

      protected Vector2 offset;
      protected Vector2 origin;
      protected Vector2 size;

      protected float translate_atspd;      // 환산 공격속도 = 1 / attack_speed

      protected float lastAttackTime = 0f;

      public Animator ch_animator;
      public Animator effect_animator;

      [Header("Layermask")]
      public LayerMask mainLayerMask;


      // 문서 상 표기 : 1초에 n번 공격.
      // 1초에 x번으로 환산 :  1/n

   
      public void Init( float dmg, float aspd, int max_count)
      {
            damage = dmg;
            attack_speed = aspd;
            attack_max_count = max_count;

            translate_atspd = 1 / attack_speed;
            
            coll = GetComponent<CapsuleCollider2D>();
            //  offset = new Vector2(coll.offset.x + transform.position.x, coll.offset.y + transform.position.y);
            offset = coll.offset;
            Debug.Log("offset " + offset);
            size = coll.size;
      }

      public void ChangeDamage(float dmg)
      {
            damage += dmg;
      }
      public void ChangeAttackSpeed(float asp)
      {
            attack_speed += asp;
      }


      void Start()
      {
        
      }

      void Update()
      {
            if (bossBase.freezeFlag == false)
            {
                  if (bossBase.attack == true)
                  {
                        if (lastAttackTime > translate_atspd)
                        {
                              bossBase.attack = false;
                              lastAttackTime = 0;

                        }
                        lastAttackTime += Time.deltaTime;
                  }
            }
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

    
      private void FixedUpdate()
      {
            origin = new Vector2(transform.localPosition.x + offset_x, transform.position.y + offset_y);

      }

      protected void Attack()
      {
            if (bossBase.Dead == true)
            {
                  return;
            }
            Debug.Log("origin " + origin);
            RaycastHit2D[] rh2ds = Physics2D.BoxCastAll(origin,size, 0f, transform.forward, 1f, mainLayerMask);
            if (rh2ds != null)
            {
                  Debug.Log("=== " + bossBase.gameObject.name + " Attack -> Damage : " + damage);

                  rh2ds = Sorting(rh2ds);

                  if (attack_max_count > rh2ds.Length)
                  {
                        // 최대 공격수가 공격범위 적보다 많을때.
                        for (int i = 0; i < rh2ds.Length; i++)
                        {
                              Attacking(rh2ds[i]);
                        }

                  }
                  else if (attack_max_count <= rh2ds.Length)
                  {
                        // 최대 공격수가 공격범위 적보다 적을때.
                        for (int i = 0; i < attack_max_count; i++)
                        {
                              Attacking(rh2ds[i]);
                        }
                  }
            }
      }

      protected void Skill()
      {
            GameObject skillObject = Instantiate(bossSkill, GameController.Instance.group.transform.position, Quaternion.identity);
            skillObject.GetComponent<BossSkill>().damage = skillDamage;
      }

      public void Attacking(RaycastHit2D rh2d)
      {
            var hitUnit = rh2d.collider.GetComponent<LivingEntity>();
            Debug.Log(hitUnit.gameObject.name + " 데미지받음 : " + damage);
            hitUnit?.OnDamage(damage, hitUnit.transform.localPosition);
            //effect 생성

            Vector2 ef_vec = hitUnit.GetComponent<LivingEntity>().effect_transform.position;
            GameObject effect = Instantiate(bossBase.effect_prefab, ef_vec, Quaternion.identity);
            Destroy(effect, 0.6f);
      }

      private void OnTriggerStay2D(Collider2D collision)
      {
            if (bossBase.attack == false && bossBase.Dead == false && bossBase.stop == false)
            {

               if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
                  {
                         Debug.Log("공격범위 감지");

                        bossBase.attack = true;
                        bossBase.animator?.SetTrigger("attack");
                        float ran = Random.Range(0f, 1f);
                        if(ran < 0.3f)
                        {
                              if(FindObjectOfType<BossSkill>() == null)
                              {
                                  
                                    Skill();
                              }
                              else
                              {
                                    Debug.Log("이미 스킬 사용중.");
                                    effect_animator.SetTrigger("attack");
                              }
                              
                        }
                        else
                        {
                              effect_animator.SetTrigger("attack");
                        }
                        
                  }
            }


      }

      private void OnDrawGizmosSelected()
      {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(origin, size);
      }
}
