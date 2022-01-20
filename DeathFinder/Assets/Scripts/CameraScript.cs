using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CameraScript : Singleton<CameraScript>
{
      private Rigidbody2D _body;

      private Vector2 _velocity;

      //private float speed;
     // private float _maxSpeed = 50f;

   //   public FadeController fader;

 //     public Vector2 prevPos = Vector2.zero;
  //    public float prevDistance = 0f;

     
      // camera shake
      private float shake_time;
      private float shake_range;

      Vector3 originPos;

      private Vector3 _nextPosition;

      // Start is called before the first frame update
      void Awake()
      {
            _body = GetComponent<Rigidbody2D>();
            if (_body == null)
            {
                  Debug.Log("_body is not exist.");
            }
            else
            {
                  _body.freezeRotation = true;
                  _body.gravityScale = 0;
            }
          //   StartCoroutine(Faiding());
      }

   /*   IEnumerator Faiding()
      {
            fader.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
            Debug.Log("faiding");
            fader.FadeOut(2f, () =>
            {
                  GameController.Instance.StartCoroutine("GameReady");
                  fader.gameObject.SetActive(false);
            });

            

            yield break;
      }

      private void Update()
      {
           // KeyInput();
      }
      private void FixedUpdate()
      {
           // MoveToTarget();
      }

     

      private void KeyInput()
      {
            speed = _maxSpeed;

            Vector2 input;
            input.x = Input.GetAxis("Horizontal");
            input.y = 0f;
            input = Vector2.ClampMagnitude(input, 1f);

            _velocity = input * speed;
      }

      public void MoveToTarget(Transform trans)
      {
            _nextPosition = trans.position;
            Vector3 direction = _nextPosition - transform.position;
            direction.Normalize();

            transform.transform.Translate(direction * (60f) * Time.deltaTime);
      }

      public IEnumerator GameOverMoving(Transform trans)
      {
            float time = 0f;
            while (time < 1.4f)
            {
                  MoveToTarget(trans);
                  time += Time.deltaTime;
                  yield return new WaitForFixedUpdate();
            }
            
      }
      */
      public void Shake(float shake_time, float shake_range)
      {
            // 카메라흔들림 옵션 조건 체크

            this.shake_range = shake_range;
            this.shake_time = shake_time;

            originPos = transform.localPosition;
            StartCoroutine(Shaking());
      }


      private IEnumerator Shaking()
      {
            // StartCoroutine(StopShaking());
            float timer = 0;
            while (timer <= shake_time)
            {

                  transform.localPosition = (Vector3)Random.insideUnitCircle * shake_range + originPos;

                  timer += Time.deltaTime;
                  yield return null;
            }
            transform.localPosition = originPos;
      }

  /*    public void GoTo(Transform transform)
      {
            this.transform.position = transform.position;
      }


      public void OnBeginDrag(PointerEventData eventData)
      {
            Debug.Log("OnBeginDrag");
            speed = _maxSpeed;

            Vector2 input;
            input.x = Input.GetAxis("Horizontal");
            input.y = 0f;
            input = Vector2.ClampMagnitude(input, 1f);

            _velocity = input * speed;
      }


      public void OnDrag()
      {

            int touchCount = Input.touchCount;
            Debug.Log("ondrag : " + touchCount);
            if (touchCount == 1)
            {
                  if (prevPos == Vector2.zero)
                  {
                        prevPos = Input.GetTouch(0).position;
                        return;
                  }

                  Vector2 dir = (Input.GetTouch(0).position - prevPos).normalized;
                  Vector3 vec = new Vector3(dir.x, 0, dir.y);

                  transform.position -= vec * speed * Time.deltaTime;
                  prevPos = Input.GetTouch(0).position;
            }
      }
      public void ExitDrag()
      {
            prevPos = Vector2.zero;
            prevDistance = 0f;
      }*/

}
