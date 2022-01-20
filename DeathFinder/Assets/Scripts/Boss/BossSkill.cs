using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
      public float damage;
      public GameObject skill;

      public float shake_time;
      public float shake_range;

      // Start is called before the first frame update
      void Start()
      {
            StartCoroutine("SkillOn");
            skill.GetComponent<BossSkillAttack>().damage = damage;
      }

      public IEnumerator SkillOn()
      {
            Debug.Log("Skill On");
            /*  yield return new WaitForSeconds(0.3f);
              GetComponent<SpriteRenderer>().enabled = false;
              yield return new WaitForSeconds(0.3f);
              GetComponent<SpriteRenderer>().enabled = true;
              yield return new WaitForSeconds(0.3f);
              GetComponent<SpriteRenderer>().enabled = false;
              yield return new WaitForSeconds(0.3f);
              GetComponent<SpriteRenderer>().enabled = true;
              yield return new WaitForSeconds(0.3f);
              GetComponent<SpriteRenderer>().enabled = false;
              yield return new WaitForSeconds(0.2f);
              GetComponent<SpriteRenderer>().enabled = true;
              yield return new WaitForSeconds(0.2f);
              GetComponent<SpriteRenderer>().enabled = false;
              yield return new WaitForSeconds(0.2f);
              GetComponent<SpriteRenderer>().enabled = true;
              yield return new WaitForSeconds(0.2f);
              GetComponent<SpriteRenderer>().enabled = false;
              yield return new WaitForSeconds(0.2f);
              GetComponent<SpriteRenderer>().enabled = true;
              yield return new WaitForSeconds(0.2f);
              GetComponent<SpriteRenderer>().enabled = false;
              yield return new WaitForSeconds(0.1f);
              GetComponent<SpriteRenderer>().enabled = true;
              yield return new WaitForSeconds(0.1f);
              GetComponent<SpriteRenderer>().enabled = false;
              yield return new WaitForSeconds(0.1f);
              GetComponent<SpriteRenderer>().enabled = true;
              yield return new WaitForSeconds(0.1f);
              GetComponent<SpriteRenderer>().enabled = false;
              yield return new WaitForSeconds(0.1f);
              GetComponent<SpriteRenderer>().enabled = true;
              yield return new WaitForSeconds(0.1f);*/
            yield return new WaitForSeconds(3.3f);
            GetComponent<SpriteRenderer>().enabled = false;

            skill.SetActive(true);
            CameraScript.Instance.Shake(shake_time, shake_range);
      }

      // Update is called once per frame
      void Update()
      {

      }
      
      
}
