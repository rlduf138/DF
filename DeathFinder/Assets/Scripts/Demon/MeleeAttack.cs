using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AttackBase
{

      private void FixedUpdate()
      {
            origin = new Vector2(transform.position.x + offset_x, transform.position.y + offset_y);

      }

      protected override void Attack()
      {
            base.Attack();


            RaycastHit2D[] rh2ds = Physics2D.BoxCastAll(origin, size, 0f, transform.forward, 1f, mainLayerMask);
            if (rh2ds != null)
            {
                  //   Debug.Log("=== " + demonBase.gameObject.name + " Attack -> Damage : " + damage);

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

      public void Attacking(RaycastHit2D rh2d)
      {
            var hitUnit = rh2d.collider.GetComponent<LivingEntity>();
            //     Debug.Log(hitUnit.gameObject.name + " 데미지받음 : " + damage);
            hitUnit?.OnDamage(damage, hitUnit.transform.localPosition);
            //  Debug.Log(hitUnit.gameObject.name + " health : " + demonBase.health);
            //effect 생성

            Vector2 ef_vec = hitUnit.GetComponent<LivingEntity>().effect_transform.position;
            GameObject effect = Instantiate(demonBase.effect_prefab, ef_vec, Quaternion.identity);
            Destroy(effect, 0.5f);
      }

      private void OnTriggerStay2D(Collider2D collision)
      {
            if (demonBase.attack == false && demonBase.Dead == false)
            {
                  if (collision.gameObject.layer == LayerMask.NameToLayer("Player2"))
                  {
                        if (demonBase.skillEnable == true)
                        {
                              // 스킬 사용 가능하면.
                              Debug.Log(demonBase.gameObject.name + " => 스킬 사용");
                              demonBase.attack = true;
                              demonBase.animator?.SetTrigger("skill");
                              demonBase.skillEnable = false;
                        }
                        else if (demonBase.skillEnable == false)
                        {
                              demonBase.attack = true;

                              demonBase.animator?.SetTrigger("attack");
                        }
                  }
            }
      }
      public void SkillObjectOn()
      {
            demonBase.skillObject.SetActive(true);
      }
      public void SkillObjectOff()
      {
            demonBase.skillObject.SetActive(false);
      }
}
