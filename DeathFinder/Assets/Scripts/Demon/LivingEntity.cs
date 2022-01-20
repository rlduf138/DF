using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LivingEntity : MonoBehaviour
{
      public float barrier;
      public float startingHealth = 100f;
      public float health;
      public bool Dead { get; protected set; }
      public bool Hit { get; protected set; }
      public bool attack;
      public event Action OnDeath;

      public Transform spriteTrans;
      public Slider hpBar;
      public Image hpBarFill;

      public Animator animator;

      public SpriteRenderer ch_sprite;

      [Header("Effect")]
      public Transform effect_transform;
      public GameObject effect_prefab;


      // 생명 활성화시 상태 리셋
      protected virtual void OnEnable()
      {
            // 사망하지 않은 상태
            Dead = false;
            // 체력을 시작 체력으로
            health = startingHealth;
            //hpBar.maxValue = startingHealth;
           // HpBarRefresh(health);
            // ch_sprite = spriteTrans.GetComponent<SpriteRenderer>();
      }

      // 데미지를 입는 기능
      public virtual void OnDamage(float damage, Vector2 hitPoint)
      {
            // 보호막 먼저 감소.
            if (barrier > 0)
            {
                  barrier -= damage;
            }
            else if (barrier == 0)
            {
                  health -= damage;
                 // HpBarRefresh(health);
            }

            if (barrier < 0)
            {
                  health += barrier;
                  //HpBarRefresh(health);
                  barrier = 0;
            }

            // 데미지만큼 체력 감소
            //체력이 0 이하 && 아직 죽지않았다면 사망처리
            if (health <= 0 && !Dead)
            {
                  Die();
                  health = 0;
            }
            Hit = true; // 맞을때 멈추게 하거나 하는 용도
            StartCoroutine(HitEffect());
      }
      public IEnumerator HitEffect()
      {
            /* ch_sprite.color = new Color(1f, 1f, 1f, 0.3f);
             yield return new WaitForSeconds(0.1f);
             ch_sprite.color = new Color(1f, 1f, 1f, 1f);
             yield return new WaitForSeconds(0.1f);
             ch_sprite.color = new Color(1f, 1f, 1f, 0.3f);
             yield return new WaitForSeconds(0.1f);
             ch_sprite.color = new Color(1f, 1f, 1f, 1f);
             yield return new WaitForSeconds(0.1f);
             ch_sprite.color = new Color(1f, 1f, 1f, 0.3f);
             yield return new WaitForSeconds(0.1f);
             ch_sprite.color = new Color(1f, 1f, 1f, 1f);
             */
            yield return new WaitForSeconds(0.8f);
            Hit = false;
            //yield return null;
      }
      // 체력 회복 기능
      public virtual void RestoreHealth(float plusHealth)
      {
            if (Dead)
            {
                  // 이미 죽었으면 회복 안함
                  return;

            }
            Debug.Log("RestoreHealth : +" + plusHealth);
            // 체력 추가
            health += plusHealth;
            HpBarRefresh(health);
      }

      // 사망 처리
      public virtual void Die()
      {
            Debug.Log("base.Die");
            // onDeath 이벤트에 등록된 메서드가 있으면 실행
            if (OnDeath != null)
            {
                  OnDeath();
            }
            // CharacterBase -> Dead 로 옮김
            if (animator != null)
            {
                  animator?.SetBool("dead", true);
            }
            // 사망상태를 참으로
            Dead = true;

      }

      public void Flip(float x)
      {
            Vector3 scale = spriteTrans.localScale;

            if (Mathf.Sign(x) != 0)
            {
                  scale.x = Mathf.Sign(x) * -1;

                  spriteTrans.localScale = scale;
            }
      }

      public void HpBarRefresh(float health)
      {
            hpBar.value = health;
            float percent = hpBar.value / hpBar.maxValue;

            if (hpBarFill != null)
            {
                  if (percent <= 0.3)
                  {
                        // 색깔 빨간색
                        hpBarFill.color = Color.red;
                  }
                  else if (percent <= 0.7)
                  {
                        // 색깔 노란색
                        hpBarFill.color = Color.yellow;
                  }
                  else if (percent <= 1)
                  {
                        // 색깔 초록색
                        hpBarFill.color = Color.green;
                  }
            }
      }
}
