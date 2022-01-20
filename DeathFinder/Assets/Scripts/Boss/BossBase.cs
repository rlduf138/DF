using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBase : LivingEntity
{
      public BossMeleeAttack meleeAttack;
      public Text bossHpText;
      public bool stop;
    
      [Header("RightTop")]
      public Sprite portrait;
      public string boss_name;

      [Header("OriginalStatus")]
      public float damage;
      public float attack_speed;
      public int attack_max_count;
      public float boss_damage;

      [Header("CurrentStatus")]
      public float cur_damage;
      public float cur_attack_speed;
      public int cur_attack_max_count;

      [Header("SkillStatus")]
      public float skill_damage;
      public float skill_delay;

      [Header("Poison")]
      public float poisonDamage;
      public float poisonTime;
      public int poisonStack = 0;
      public GameObject poisonEffect;

      [Header("ManaBoom")]
      public GameObject manaBoom;
      private float boomDamage;

      [Header("Weakness")]
      public bool weaknessFlag;
      private float weaknessPercent;
      public GameObject weaknessEffect;

      [Header("Freeze")]
      public bool freezeFlag;
      private float freezePercent;
      public GameObject freezeEffect;

      [Header("Condemn")]
      public bool condemnFlag;
      private float condemnPercent;
      public GameObject condemnEffect;

      private void Awake()
      {
            cur_damage = damage;
            cur_attack_speed = attack_speed;
            cur_attack_max_count = attack_max_count;
            
           /* Debug.Log("BossBase Start");
            OnDeath += Death;

            GameController.Instance.InitBoss(this);
            meleeAttack.Init(damage, attack_speed, attack_max_count);
            bossHpText.text = string.Format("{0:D}", (int)health) + "/" + startingHealth.ToString();*/
      }

      void Start()
      {
            Debug.Log("BossBase Start");
            OnDeath += Death;
            
            GameController.Instance.InitBoss(this);
            meleeAttack.Init( damage, attack_speed, attack_max_count);
            meleeAttack.skillDamage = skill_damage;
            bossHpText.text = string.Format("{0:D}", (int)health) + "/" + startingHealth.ToString();

            stop = true;
      }
      public void InitBoss()
      {
            Debug.Log("InitBoss");
            meleeAttack.Init(damage, attack_speed, attack_max_count);
            health = startingHealth;
            bossHpText.text = string.Format("{0:D}", (int)health) + "/" + startingHealth.ToString();
      }

      public void Death()
      {
            GameController.Instance.StartCoroutine("GameWin");
      }

      public IEnumerator PoisonMist()
      {
            poisonEffect.SetActive(true);
            float poison_currentTime = 0f;
            poisonStack++;
            while (poisonTime > poison_currentTime)
            {
                  OnDamage(poisonDamage, Vector2.zero);

                  yield return new WaitForSeconds(1f);
                  poison_currentTime += 1f;
            }
            poisonStack--;
            if(poisonStack == 0)
            {
                  poisonEffect.SetActive(false);
            }
      }
      public IEnumerator ManaBoom(int damage)
      {
            boomDamage = damage;
            Debug.Log("ManaBoomDamage : " + boomDamage);
            manaBoom.SetActive(true);

            yield return new WaitForSeconds(0.3f);
            ManaBoomAttack();
            yield return new WaitForSeconds(0.34f);
            manaBoom.SetActive(false);
      }
      public void ManaBoomAttack()
      {
            OnDamage(boomDamage, Vector2.zero);
      }
      // =================== Freeze ===================================
      public void FreezeOn(float percent)
      {
            Debug.Log("FreezeOn");
            freezePercent = percent;
            if(freezeFlag == false)
            {
                  freezeEffect.SetActive(true);
                  freezeFlag = true;

            }
      }
      public void FreezeOff()
      {
            Debug.Log("FreezeOff");
            if (freezeFlag == true)
            {
                  freezeEffect.SetActive(false);
                  freezeFlag = false;

            }
      }

      // ==================== Condemn ===============================
      public void CondemnOn(float percent)
      {
            Debug.Log("CondemnOn");
            condemnPercent = percent;
            if (condemnFlag == false)
            {
                  condemnEffect.SetActive(true);
                  condemnFlag = true;

            }
      }
      public void CondemnOff()
      {
            Debug.Log("CondemnOff");
            if (condemnFlag == true)
            {
                  condemnEffect.SetActive(false);
                  condemnFlag = false;

            }
      }

      // ================== Weakness =================================
      public void WeaknessOn(float percent)
      {
            weaknessPercent = percent;
            Debug.Log("WeaknessOn");
            if(weaknessFlag == false)
            {
                  weaknessEffect.SetActive(true);
                  weaknessFlag = true;

                  cur_damage -= damage * weaknessPercent;
                //  cur_attack_speed -= attack_speed * cursePercent;

                  meleeAttack.Init( cur_damage, cur_attack_speed, cur_attack_max_count);
            }
      }
      public void WeaknessOff()
      {
            Debug.Log("WeaknessOff");
            if(weaknessFlag == true)
            {
                  weaknessEffect.SetActive(false);
                  weaknessFlag = false;

                  cur_damage += damage * weaknessPercent;
                 // cur_attack_speed += attack_speed * cursePercent;

                  meleeAttack.Init( cur_damage, cur_attack_speed, cur_attack_max_count);
            }
      }
      public override void OnDamage(float damage, Vector2 hitPoint)
      {
            base.OnDamage(damage, hitPoint);
            if(freezeFlag == true)
            {
                  health += damage*freezePercent/100;
            }
            if(condemnFlag == true)
            {
                  health -= damage * condemnPercent / 100;
            }
            bossHpText.text = string.Format("{0:D}", (int)health) + "/" + startingHealth.ToString();
            HpBarRefresh(health);
            //bossHpText.text = health.ToString() + "/" + startingHealth.ToString();
      }
      public override void RestoreHealth(float newHealth)
      {
            base.RestoreHealth(newHealth);
            bossHpText.text = string.Format("{0:D}", (int)health) + "/" + startingHealth.ToString();
      }
}
