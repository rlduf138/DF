using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
      

      public MinionBase minionBase;
      public GameObject projectile;
      public Transform firePos;

      protected float damage;
      protected float attack_speed;
      protected float projectile_speed;

      protected float translate_atspd;
      protected float lastAttackTime = 0f;

      public void SetSpeed(float speed)
      {
            projectile_speed = speed;
      }
      public void InitMinionAttack(float dmg, float aspd)
      {
            Debug.Log("InitMinionAttack");
            damage = dmg;
            attack_speed = aspd;

            translate_atspd = 1 / attack_speed;
      }

      // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {
            // if (minionBase.attack == true)
            //  {
            if (!minionBase.Dead)
            {
                  if (minionBase.assemble == false)
                  {
                        if (minionBase.can_attack == true)
                        {
                              if (lastAttackTime > translate_atspd)
                              {
                                    minionBase.attack = false;
                                    lastAttackTime = 0;

                                    Attack();
                              }
                              lastAttackTime += Time.deltaTime;
                        }
                  }
            }
      }

      internal void ChangeAttackSpeed(float cur_attack_speed)
      {
            throw new NotImplementedException();
      }

      internal void ChangeDamage(float cur_damage)
      {
            throw new NotImplementedException();
      }

      public void Attack()
      {
            minionBase.animator.SetTrigger("attack");
            GameObject gameObject = Instantiate(projectile, firePos.position, Quaternion.identity);
            gameObject.GetComponent<Projectile>().InitProjectile(damage, projectile_speed);
      }
}
