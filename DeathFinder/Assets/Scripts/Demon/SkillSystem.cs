using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
      public DemonBase demonBase;
      public MeleeAttack meleeAttack;
      public float skillDamage;
      public int skillHitCount;

      public GameObject particle;

      void Start()
      {
            skillDamage = demonBase.skillDamage;
            skillHitCount = demonBase.skillHitCount;
      }

      void Update()
      {

      }

      private void Attack()
      {
            if (demonBase.Dead == true)
            {
                  return;
            }

            RaycastHit2D[] rh2ds = Physics2D.BoxCastAll(meleeAttack.origin, meleeAttack.size, 0f, transform.forward, 1f, meleeAttack.mainLayerMask);
            if (rh2ds != null)
            {
                  //   Debug.Log("=== " + demonBase.gameObject.name + " Attack -> Damage : " + damage);

                 
                  if (skillHitCount > rh2ds.Length)
                  {
                        // 최대 공격수가 공격범위 적보다 많을때.
                        for (int i = 0; i < rh2ds.Length; i++)
                        {
                              var hitUnit = rh2ds[i].collider.GetComponent<LivingEntity>();
                              hitUnit?.OnDamage(skillDamage, hitUnit.transform.localPosition);

                              Vector2 ef_vec = new Vector2(hitUnit.GetComponent<LivingEntity>().effect_transform.position.x, demonBase.transform.position.y);
                              GameObject effect = Instantiate(particle, ef_vec, Quaternion.identity);
                              Destroy(effect, 0.5f);
                        }

                  }
                  else if (skillHitCount <= rh2ds.Length)
                  {
                        // 최대 공격수가 공격범위 적보다 적을때.
                        for (int i = 0; i < skillHitCount; i++)
                        {
                              var hitUnit = rh2ds[i].collider.GetComponent<LivingEntity>();
                              hitUnit?.OnDamage(skillDamage, hitUnit.transform.localPosition);

                              Vector2 ef_vec = new Vector2(hitUnit.GetComponent<LivingEntity>().effect_transform.position.x, demonBase.transform.position.y);
                              GameObject effect = Instantiate(particle, ef_vec, Quaternion.identity);
                              Destroy(effect, 0.5f);
                        }
                  }
            }
            
      }
}
