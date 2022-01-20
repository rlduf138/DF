using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorParticle : MonoBehaviour
{
      // 12에서  -1.72까지
      // direction = 0.8 0.5 0
      
      public float min_x;     //-15
      public float max_x;     //30
      public float speed;

      public Vector3 direction;
      private ParticleSystem particle;

      private void Start()
      {
            particle = GetComponentInChildren<ParticleSystem>();
            RandomPosition();
            StartCoroutine(MeteorDrop());
      }

      private void RandomPosition()
      {
            float ran = Random.Range(min_x, max_x);
            transform.position = new Vector2(ran, 12f);
      }
      
      private IEnumerator MeteorDrop()
      {
            while (transform.position.y >= -1.72f)
            {
                  transform.transform.Translate(direction * speed * Time.deltaTime);

                  yield return new WaitForFixedUpdate();
            }
            StartCoroutine("MeteorEnd");
      }
      private IEnumerator MeteorEnd()
      {
            // 파티클 멈춘 후 랜덤위치에 파티클 이동.
            yield return new WaitForSeconds(0.5f);
            particle.Stop();
            RandomPosition();

            float ran = Random.Range(1f, 3f);

            // 파티클 시작 후 메테오드랍
            yield return new WaitForSeconds(ran);
            particle.Play();
            StartCoroutine(MeteorDrop());

      }
}
