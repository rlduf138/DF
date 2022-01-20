using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaController : Singleton<SeaController>
{
      public FadeController fader;

      public Animator ggasiAnimator_front;
      public Animator ggasiAnimator_back;

      public GameObject rellic;
      public GameObject cardCanvas;

      // Start is called before the first frame update
      void Start()
      {
            fader.InitColor(new Color(0f, 0f, 0f, 1f));
            fader.FadeOut(1f, () =>
            {
                  ggasiAnimator_front.gameObject.SetActive(true);
                  ggasiAnimator_back.gameObject.SetActive(true);
                  fader.gameObject.SetActive(false);
            });
      }

      // Update is called once per frame
      void Update()
      {

      }

      public void OnGgasi01()
      {
            // 유물 등장
            ggasiAnimator_front.SetBool("ggasi01", true);
            ggasiAnimator_back.SetBool("ggasi01", true);
            ggasiAnimator_front.SetBool("ggasi02", false);
            ggasiAnimator_back.SetBool("ggasi02", false);
            ggasiAnimator_front.SetBool("ggasi03", false);
            ggasiAnimator_back.SetBool("ggasi03", false);
      }
      public void OnGgasi02()
      {
            // 카드 선택
            ggasiAnimator_front.SetBool("ggasi01", false);
            ggasiAnimator_back.SetBool("ggasi01", false);
            ggasiAnimator_front.SetBool("ggasi02", true);
            ggasiAnimator_back.SetBool("ggasi02", true);
            ggasiAnimator_front.SetBool("ggasi03", false);
            ggasiAnimator_back.SetBool("ggasi03", false);
      }
      public void OnGgasi03()
      {
            // 특성 선택
            ggasiAnimator_front.SetBool("ggasi01", false);
            ggasiAnimator_back.SetBool("ggasi01", false);
            ggasiAnimator_front.SetBool("ggasi02", false);
            ggasiAnimator_back.SetBool("ggasi02", false);
            ggasiAnimator_front.SetBool("ggasi03", true);
            ggasiAnimator_back.SetBool("ggasi03", true);
      }

      public void CardCanvasOn()
      {
            cardCanvas.SetActive(true);
      }
      public void RellicOn()
      {
            rellic.SetActive(true);
      }

      public IEnumerator ActiveScene()
      {
            fader.gameObject.SetActive(true);
            fader.FadeIn(1f, () =>
            {

                  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StageSelect");
            });
            yield break;
      }
      
}
