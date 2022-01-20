using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;
using TMPro;

public class Card : MonoBehaviour
{
      public bool mulligan;

      public bool mulligan_select = false;

      public int card_number;

      [Header("Animation")]
      public Sprite[] burn_sprites;
      public float burn_animationSpeed;
      public Image background_image;
      public Image burn_image;
      public Image burn_image2;
      public Sprite[] glowSprites;
      public float glow_animationSpeed;
      public Image glow_image;

      [Header("CardInfo")]
      public GameObject magic_prefab;
      public Image magic_image;
    //  public Text name_text;
     // public Text explain_text;
     // public Text cost_text;
      public Text atk_text;
      public Text hp_text;
      public TextMeshProUGUI name_text;
      public TextMeshProUGUI explain_text;
      public TextMeshProUGUI cost_text;

      float prev_x;
      float prev_y;

      float new_y = 110;


      Vector3 _sortingNextPosition;

      Vector3 _nextPosition;
      public bool edge_flag;



      // Start is called before the first frame update
      void Awake()
      {
            prev_x = transform.localPosition.x;
            prev_y = transform.localPosition.y;

      }
      void Start()
      {
            // StartCoroutine("EdgeEffect");
            burn_image.enabled = false;
            burn_image.GetComponent<Mask>().enabled = false;
            burn_image2.enabled = false;
      }
      public void GotoPrevPosition()
      {
            transform.localPosition = new Vector2(prev_x, prev_y);
      }

      public void CheckEdge()
      {
           /* if (magic_prefab.GetComponent<MagicBase>().cost <= data._currentMana)
            {
                  // 현재 마나가 코스트보다 같거나 크면.

                  if (edge_flag == false)
                  {
                        edge_flag = true;
                        // 카드 테두리 빛.
                       // glow_image.enabled = true;
                        try
                        {
                              //     StartCoroutine("EdgeEffect");
                        }
                        catch (Exception e)
                        {
                              Debug.Log(e);
                        }

                  }
            }
            else
            {
                  //       StopCoroutine("EdgeEffect");
                  edge_flag = false;
               //   glow_image.enabled = false;


            }*/
      }

      internal void SetMagic(GameObject magic)
      {
            magic_prefab = magic;
            magic_image.sprite = magic_prefab.GetComponent<MagicBase>().magicSprite;
          //  magic_image.SetNativeSize();
            name_text.text = magic_prefab.GetComponent<MagicBase>().magicName;
            explain_text.text = magic_prefab.GetComponent<MagicBase>().magicExplain;


            cost_text.text = magic_prefab.GetComponent<MagicBase>().cost.ToString();

            //explain_text.resizeTextForBestFit = true;

            //CheckEdge();
      }

      // =========================== 카드 불타는 애니메이션 =======================

      public IEnumerator CardBurnAnimation()
      {
            burn_image.enabled = true;
            burn_image.GetComponent<Mask>().enabled = true;
            burn_image2.enabled = true;

            glow_image.enabled = false;

            for (int i = 0; i < burn_sprites.Length; i++)
            {
                  burn_image.sprite = burn_sprites[i];
                  burn_image2.sprite = burn_sprites[i];

                  yield return new WaitForSeconds(burn_animationSpeed);

            }
            //burn_image.gameObject.SetActive(false);
      }

      // ============================ Game중 =================================
      public IEnumerator CardSorting(Transform _transform)
      {
            _sortingNextPosition = _transform.position;

            Vector3 direction = _sortingNextPosition - transform.position;

            float current_time = 0f;

            while (current_time < 0.1)
            {
                  transform.transform.Translate(direction * 10f * Time.deltaTime);
                  yield return new WaitForFixedUpdate();
                  current_time += Time.deltaTime;
            }

            transform.SetParent(_transform);
            transform.localPosition = new Vector2(0, 0);

            yield return null;
      }

      public IEnumerator EdgeEffect()
      {
            Debug.Log("EdgeEffect card_number : " + card_number);
            edge_flag = true;
            glow_image.enabled = true;

            int i = 0;
            while (true)
            {
                  // 애니메이션 추가.
                  if (i == glowSprites.Length)
                  {
                        i = 0;
                  }

                  glow_image.sprite = glowSprites[i];
                  i++;

                  yield return new WaitForSeconds(glow_animationSpeed);

            }
      }

      public IEnumerator ZoomInCard()
      {
            float currentTime = 0f;

            while (transform.localPosition.y < new_y)
            {

                  _nextPosition = new Vector2(prev_x, new_y);
                  yield return new WaitForFixedUpdate();

                  Vector3 direction = _nextPosition - transform.localPosition;
                  direction.Normalize();

                  transform.transform.Translate(direction * 60f * Time.deltaTime);
                  currentTime += Time.deltaTime;
            }
            //transform.localScale = new Vector2(1.5f, 1.5f);

            CardController.Instance.PanelsOn();
      }
      public void ZoomOutCard()
      {
            StopCoroutine("ZoomInCard");
            transform.localScale = new Vector2(1.5f, 1.5f);
            transform.localPosition = new Vector2(prev_x, prev_y);
            CardController.Instance.PanelsOff();
      }

      public void SummonCard()
      {
           // transform.localScale = new Vector2(1f, 1f);


            if (ManaController.Instance.UseMana(magic_prefab.GetComponent<MagicBase>().cost) == true)
            {
                  StartCoroutine("Summon");
            }
            else ZoomOutCard();

      }

      public IEnumerator Summon()
      {
            Debug.Log("Summon");
            CardController.Instance.PanelsOn();
            //     GameController.Instance.SpawnDemon(magic_prefab, 1);
            Instantiate(magic_prefab);

            StartCoroutine("CardBurnAnimation");
            yield return new WaitForSeconds(0.45f);



            name_text.text = "none";

            CardController.Instance.StartCoroutine("Sorting", card_number);
            yield return new WaitForSeconds(0.1f);

            //CardController.Instance.PanelsOn();
      }

      public bool CheckWhere(PointerEventData eventData)
      {
            if (eventData.pointerEnter == CardController.Instance.cancel_panel)
            {
                  return false;
            }
            else if (eventData.pointerEnter == CardController.Instance.summon_panel)
            {
                  return true;
            }
            return false;
      }

      // =============================== 클릭 이벤트 =============================

      public void PointerDown()
      {
            if (Input.touchCount <= 1)
            {
                  CardController.Instance.PanelsOn();

                  Debug.Log("PointerDown");
                  //   StartCoroutine("ZoomInCard");
                  // 우측에 카드 확대해서 보여주기.
                 // CardController.Instance.ViewCardInfo(magic_prefab);
            }
      }
      public void PointerUp()
      {
            Debug.Log(Input.touchCount);

            //   ZoomOutCard();
          //  CardController.Instance.OffCardInfo();
            CardController.Instance.PanelsOff();
      }

      public void OnDrag(PointerEventData eventData)
      {
            if (Input.touchCount <= 1)
            {
                  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                  RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector3.zero);
                  transform.position = ray.origin;
                  
                  // 카드가 작아지고, 손꾸락을 따라간다.
                  transform.localScale = new Vector2(2f, 2f);

            }
      }


      public void OnBeginDrag(PointerEventData eventData)
      {
            if (Input.touchCount <= 1)
            {
                  CardController.Instance.PanelsOn();
                  StopCoroutine("ZoomInCard");
            }
      }

      public void OnEndDrag(PointerEventData eventData)
      {
            if (Input.touchCount <= 1)
            {
                  Debug.Log("EndDrag");

                  // StopCoroutine("EdgeEffect");
                  edge_flag = false;
                  //glow_image.gameObject.SetActive(false);

                  if (CheckWhere(eventData) == true && CardController.Instance.CheckPossible(magic_prefab))
                  {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector3.zero);
                        transform.position = ray.origin;
                        // 소환
                        SummonCard();
                  }
                  else
                  {
                        ZoomOutCard();
                  }
            }
      }

      // ============================ Mulligen System ==========================

      public void CardClick()
      {
            if (mulligan_select == true)
            {
                  // 선택 해제
                  GetComponent<Image>().color = Color.white;
                  mulligan_select = false;
            }
            else if (mulligan_select == false)
            {
                  // 선택
                  GetComponent<Image>().color = Color.red;
                  mulligan_select = true;
            }
            MulliganController.Instance.RefreshMulligan();
      }
      public void InitMulligan()
      {
            mulligan_select = false;
            GetComponent<Image>().color = Color.white;
      }


}
