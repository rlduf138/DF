using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonBase : LivingEntity
{
      public int index;
      public MeleeAttack meleeAttack;
      private DemonScan demonScan;

      [Header("LeftTop")]
      public Sprite portrait;

      [Header("card_info")]
      public Sprite card_img;
      public string demon_name;
      public string explain;

      [Header("ScanRange")]
      public float sc_xRadius;
      public float sc_yRadius;
      public float offset_x;
      public float offset_y;
      
      [Header("attackRange")]
      public float ar_xRadius;
      public float ar_yRadius;

      [Header("Skill")]
      public bool skillExist;       // 스킬이 존재하는지.
      public GameObject skillObject;
      public int skillHitCount;
      public float coolTime;        // 쿨타임
      public float skillDamage;     // 스킬 데미지
      public bool skillEnable;      // 스킬 사용가능 여부
      private float skillCurrentTime;
      public Slider skillSlider;

      [Header("OriginalStatus")]
      public float speed;
      public float damage;
      public float attack_speed;
      public int attack_max_count;
      public float boss_damage;

      [Header("CurrentStatus")]
      public float cur_speed;
      public float cur_damage;
      public float cur_attack_speed;
      public int cur_attack_max_count;

      [Header("Magic")]
      public bool berserker;
      public bool barrier_flag;
      public GameObject barrierEffect;
      public GameObject berserkerEffect;
      public GameObject healEffect;
    

      [Header("ETC")]
      public Vector3 _nextPosition;
      private Rigidbody2D _body;

     
      private Transform first_pos;
      public bool stop;
      public bool first_pos_flag = false;
      public bool scan_flag = false;
      private int ran;
      // Start is called before the first frame update

  // =================== Init Status =============================
      private void Awake()
      {
             cur_speed = speed;
            cur_damage = damage;
            cur_attack_speed = attack_speed;
            cur_attack_max_count = attack_max_count;
            meleeAttack.InitMeleeAttack(this, damage, attack_speed,attack_max_count, ar_xRadius, ar_yRadius, offset_x, offset_y);

            if (tag == "Player")
            {
                  stop = true;
            }
      }

      // 현재 스텟에 추가 수치
      public void ChangeDamage(float dmg)
      {
            cur_damage += dmg;
            meleeAttack.ChangeDamage(cur_damage);
      }
      public void ChangeAttackSpeed(float asp)
      {
            cur_attack_speed += asp;
            meleeAttack.ChangeAttackSpeed(cur_attack_speed);
      }
      public void ChangeSpeed(float spd)
      {
            cur_speed += spd;
      }
      public void ChangeHp(float _hp)
      {
            health += _hp; 
      }
     

      void Start()
      {
            OnDeath += Death;
            
            _body = GetComponent<Rigidbody2D>();
           skillSlider.maxValue = coolTime;
            ran = Random.Range(0, 4);

            if (tag == "Player")
            {
                  //   _nextPosition = GameController.Instance.p1_firstpos[ran].transform.localPosition;
                  first_pos_flag = true;
            }
            else if (tag == "Player2")
            {
                  stop = false;
                  _nextPosition = GameController.Instance.p2_firstpos[ran].transform.localPosition;
            }
      }
      void Update()
      {
            if (skillEnable == false && skillExist == true)
            {
                  skillCurrentTime += Time.deltaTime;
                  skillSlider.value = skillCurrentTime;
                  if (skillCurrentTime >= coolTime)
                  {
                        // 스킬 사용 가능.
                        skillEnable = true;
                        skillCurrentTime = 0f;
                  }
            }
      }

      private void FixedUpdate()
      {
            Move();

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

           
      }

      public void ArriveBoss()
      {
            Dead = true;
            Death();
      }
      
      public void Death()
      {
            if (gameObject.tag == "Player")
            {
                  GetComponent<CapsuleCollider2D>().enabled = false;
                  GameController.Instance.DeleteCharacter(index);
                  Destroy(gameObject, 1f);
            }
            else if (gameObject.tag == "Player2")
            {
                  GetComponent<CapsuleCollider2D>().enabled = false;
                  GameController.Instance.DeleteCharacter2(index);
                  Destroy(gameObject, 1f);
            }
      }
      public override void OnDamage(float damage, Vector2 hitPoint)
      {
            base.OnDamage(damage, hitPoint);
            if(barrier == 0)
            {
                  barrierEffect.SetActive(false);
            }
            animator.SetTrigger("Hit");
      }

      private void Move()
      {
            if (!Dead && !Hit && !attack &&!stop)
            {
                  Vector3 direction = _nextPosition - transform.position;
                  direction.Normalize();

                  Flip(direction.x);  // 좌우 반전

                  animator.SetTrigger("walk");

                  //Vector3 velocity = direction * speed;
                  transform.transform.Translate(direction * cur_speed * Time.deltaTime);

                  _nextPosition = new Vector2(GameController.Instance.p2_spawn.localPosition.x + 3, transform.localPosition.y);

            }
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

                  meleeAttack.InitMeleeAttack(this, cur_damage, cur_attack_speed, attack_max_count, ar_xRadius, ar_yRadius, offset_x, offset_y);
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

                  meleeAttack.InitMeleeAttack(this, cur_damage, cur_attack_speed, attack_max_count, ar_xRadius, ar_yRadius, offset_x, offset_y);
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
