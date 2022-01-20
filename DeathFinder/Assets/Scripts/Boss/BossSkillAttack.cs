using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillAttack : MonoBehaviour
{
      public float damage;
      public GameObject bossSkill;
      public GameObject effect_prefab;

      private void Start()
      {
            Destroy(bossSkill, 0.8f);
      }

      private void OnTriggerEnter2D(Collider2D collision)
      {
            if (collision.tag == "Player")
            {
                  Debug.Log("SkillTriggerEnter");
                  collision.GetComponent<LivingEntity>().OnDamage(damage, collision.transform.position);
                //  Destroy(bossSkill, 0.8f);

                  Vector2 ef_vec = collision.GetComponent<LivingEntity>().effect_transform.position;
                  GameObject effect = Instantiate(effect_prefab, ef_vec, Quaternion.identity);
                  Destroy(effect, 0.6f);
            }
      }
}
