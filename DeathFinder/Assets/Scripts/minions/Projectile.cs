using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
      public float range;
      public float damage = 1f;
      public float speed = 0f;
      public float lifeTime = 1f;

      public Rigidbody2D _rb;

      public GameObject effect_prefab;
      private Animator animator;
      bool fire_flag;

      void Start()
      {
            _rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
      }
      public void InitProjectile(float damage, float speed)
      {
            this.damage = damage;
            this.speed = Mathf.Clamp(this.speed + speed, 1f, 1000f);
      }

      public void Fire()
      {
            speed = 20f;
            fire_flag = true;
            animator.SetTrigger("fire");
      }


      void FixedUpdate()
      {
            if(fire_flag == true)
            _rb.velocity = new Vector2(speed, 0);
      }

    
      private void Update()
      {

            if (lifeTime <= 0)
            {
                  Destroy(gameObject);
            }
            lifeTime -= Time.deltaTime;
      }

      protected void OnTriggerEnter2D(Collider2D collision)
      {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player2"))
            {
                  var hitUnit = collision.GetComponent<LivingEntity>();
                  hitUnit?.OnDamage(damage, hitUnit.transform.position);
                  speed = 0;
                  animator.SetTrigger("explode");
                  Destroy(gameObject, 0.5f);
            }

            /* if(collision.gameObject.layer == LayerMask.NameToLayer("Player") ||
                  collision.gameObject.layer == LayerMask.NameToLayer("Player2"))
             {
                   var hitUnit = collision.GetComponent<LivingEntity>();
                   hitUnit?.OnDamage(damage, hitUnit.transform.position);

                   Vector2 ef_vec = new Vector2(hitUnit.transform.position.x, effect_prefab.transform.position.y);
                   GameObject effect = Instantiate(effect_prefab, ef_vec, Quaternion.identity);
                   Destroy(effect, 0.5f);

                   Destroy(gameObject);
             }*/
      }


      protected void OnDrawGizmosSeleted()
      {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
      }


}
