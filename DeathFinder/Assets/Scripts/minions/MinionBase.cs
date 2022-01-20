using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBase : LivingEntity
{
      [Header("MinionBase")]
      public int index;
      public RangeAttack rangeAttack;

      [Header("OriginalStatus")]
      public float speed;
      public float damage;
      public float attack_speed;
      public float projectile_speed;

      [Header("CurrentStatus")]
      public float cur_speed;
      public float cur_damage;
      public float cur_attack_speed;

      [Header("Magic")]
      public bool berserker;
      public bool barrier_flag;
      public GameObject barrierEffect;
      public GameObject berserkerEffect;
      public GameObject healEffect;
      public GameObject sacrificeEffect;

      [Header("Target")]
      public GameObject boss;
      public GameObject myPosition;

      [Header("ETC")]
      public Vector3 _nextPosition;
      private Rigidbody2D _body;
      public bool stop;
      public bool can_attack = false;
      public bool assemble = false; // false = 공격가능, true = 공격 중지

      private void Awake()
      {
            cur_speed = speed;
            cur_damage = damage;
            cur_attack_speed = attack_speed;

            
          
      }

      // 현재 스텟에 추가 수치
      public void ChangeDamage(float dmg)
      {
            cur_damage += dmg;
            rangeAttack.ChangeDamage(cur_damage);
      }
      public void ChangeAttackSpeed(float asp)
      {
            cur_attack_speed += asp;
            rangeAttack.ChangeAttackSpeed(cur_attack_speed);
      }
      public void ChangeSpeed(float spd)
      {
            cur_speed += spd;
      }
      public void ChangeHp(float _hp)
      {
            health += _hp;
      }

      // Start is called before the first frame update
      void Start()
      {
            OnDeath += Death;

            _body = GetComponent<Rigidbody2D>();
            rangeAttack.InitMinionAttack(cur_damage, cur_attack_speed);
            rangeAttack.SetSpeed(projectile_speed);
            // boss = GameController.Instance.boss_list[0];
            //StartCoroutine("Moving");
            hpBar.maxValue = startingHealth;
            hpBar.value = health;
            HpBarRefresh(health);

            stop = true;
      }

      // Update is called once per frame
      void Update()
      {

      }
      private void FixedUpdate()
      {
            Move();

          //  transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);


      }

      private void Move()
      {
            if (!Dead && !Hit && !attack && !stop)
            {
                 if (transform.localPosition.x  - myPosition.transform.position.x < -0.5f
                        || transform.localPosition.x - myPosition.transform.position.x > 0.5f)
                  {
                        can_attack = false;
                        Vector3 direction = _nextPosition - transform.position;
                        direction.Normalize();

                        Flip(-direction.x);  // 좌우 반전

                        animator.SetBool("walk", true);

                        //Vector3 velocity = direction * speed;
                        transform.transform.Translate(direction * cur_speed * Time.deltaTime);

                        //   _nextPosition = new Vector2(GameController.Instance.p2_spawn.localPosition.x + 3, transform.localPosition.y);
                        //    _nextPosition = new Vector2(myPosition.transform.localPosition.x + 1f, myPosition.transform.localPosition.y);
                        _nextPosition = myPosition.transform.position;
                  }
                  else
                  {
                        Flip(-1);
                        animator.SetBool("walk", false);
                        can_attack = true;
                  }
            }
      }
      public override void OnDamage(float damage, Vector2 hitPoint)
      {
            base.OnDamage(damage, hitPoint);
            if(barrier == 0)
            {
                  barrierEffect.SetActive(false);
            }
            HpBarRefresh(health);
            animator.SetTrigger("hit");
            StartCoroutine("KnockBack");
      }
      public IEnumerator KnockBack()
      {
            float time = 0f;
             //Vector2 force = new Vector2(-1, 0);
          
            Vector3 direction = new Vector3(-1, 0, 0);
            direction.Normalize();

            while (time < 0.3f)
            {

                   transform.transform.Translate(direction * 3 * Time.deltaTime);

                  //_body.AddForce(force, ForceMode2D.Impulse);

                  yield return new WaitForFixedUpdate();
        
                  time += Time.deltaTime;
               }
         //   Vector2 force2 = new Vector2(1, 0);
          // _body.AddForce(force2);
            yield return null;
      }
      public IEnumerator Moving()
      {
            while(transform.localPosition.x < myPosition.transform.localPosition.x)
            {
                  yield return new WaitForFixedUpdate();
                  Debug.Log("MOVING");
                  if (!Dead && !Hit && !attack && !stop)
                  {
                        Vector3 direction = _nextPosition - transform.position;
                        direction.Normalize();

                        Flip(-direction.x);  // 좌우 반전

                        animator.SetTrigger("walk");

                        //Vector3 velocity = direction * speed;
                        transform.transform.Translate(direction * cur_speed * Time.deltaTime);

                        // _nextPosition = new Vector2(GameController.Instance.p2_spawn.localPosition.x + 3, transform.localPosition.y);
                        _nextPosition = myPosition.transform.localPosition;
                  }
            }

            
      }
      public void Death()
      {
          //  if (gameObject.tag == "Player")
           // {
                  GetComponent<CapsuleCollider2D>().enabled = false;
                  GameController.Instance.DeleteCharacter(index);
                  gameObject.tag = "Untagged";
                  //Destroy(gameObject, 2f);
           // }
           
      }
      public IEnumerator Sacrifice()
      {
            Debug.Log("Sacrifice Coroutine");
            OnDamage(999, transform.position);
            sacrificeEffect.SetActive(true);
            yield return new WaitForSeconds(1f);
            sacrificeEffect.SetActive(false);
      }

      public IEnumerator Revive()
      {
            animator.SetBool("dead", false);
            yield return new WaitForSeconds(1f);

            health = startingHealth * 30 / 100;
            GetComponent<CapsuleCollider2D>().enabled = true;
            gameObject.tag = "Player";
            GameController.Instance.p1_dead[index] = false;
            Dead = false;
            
      }

      // ======================== Magic Card =============================

      public void BerserkerOn()
      {
            Debug.Log("BerserkerOn");
            if (berserker == false)
            {
                  berserker = true;
                  berserkerEffect.SetActive(true);

                  cur_speed += speed * 0.2f;
                  cur_damage += damage * 0.2f;
                  cur_attack_speed += attack_speed * 0.2f;

                  rangeAttack.InitMinionAttack( cur_damage, cur_attack_speed);
            }
      }
      public void BerserkerOff()
      {
            Debug.Log("BerserkerOff");
            if (berserker == true)
            {
                  berserker = false;
                  berserkerEffect.SetActive(false);

                  cur_speed -= speed * 0.2f;
                  cur_damage -= damage * 0.2f;
                  cur_attack_speed -= attack_speed * 0.2f;

                  rangeAttack.InitMinionAttack( cur_damage, cur_attack_speed);
            }
      }
      public void BarrierOn()
      {
            barrier = 100f;
            barrierEffect.SetActive(true);
      }
      public void BarrierOff()
      {
            barrier = 0f;
            barrierEffect.SetActive(false);
      }
      public IEnumerator HealEffectOn()
      {
            healEffect.SetActive(true);
            yield return new WaitForSeconds(1.1f);
            healEffect.SetActive(false);
      }

     
}
