using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class StageController : Singleton<StageController>
{
      public FadeController fadeController;
      public bool cameraMoveFlag;

      public GameObject[] selectGroups;
      public GameObject[] stageButtons;

      public Text stageNumber;
      public GameObject tutorialObject;
      public Text tutorialText;
      public int lessDeckCount;

      [Header("Selected")]
      public StageButton selectedStage;
      public float se_BossHp;
      public float se_BossDamage;
      public int se_BossNumber;

      [Header("Button")]
      public Button startButton;
      
      // Start is called before the first frame update
      void Start()
      {

            /* if (!PlayerPrefs.HasKey("Stage"))
             {
                   // 스테이지 키가 없으면.
                   Debug.Log("스테이지 키 없음");
                   PlayerPrefs.SetInt("Stage", 1);
             }*/
            int stage = PlayerPrefs.GetInt("Stage");
            Debug.Log("stage : " + stage);
            StageButton _stageButton = stageButtons[stage - 1].GetComponent<StageButton>();
            // 처음에 선택되어있는 스테이지 맞춤.
            ChangeSelectedStage(_stageButton);
            _stageButton.CameraTeleport();


            for (int i =0; i <= PlayerPrefs.GetInt("ClearStage"); i++)
            {
                  // 클리어 한 다음 스테이지까지 버튼 활성화.
                  stageButtons[i].GetComponent<PolygonCollider2D>().enabled = true;
                  Color color = stageButtons[i].GetComponent<SpriteRenderer>().color;
                  color.a = 1f;
                  stageButtons[i].GetComponent<SpriteRenderer>().color = color;
            }

           
            //       stageButtons[stage + 1].GetComponent<StageButton>().OnMouseUp();
     

            // 최소 덱 수 부족
            if (PlayerPrefs.GetInt("DeckCount") < lessDeckCount)
            {
                  tutorialObject.SetActive(true);
                  tutorialText.text = "덱을 최소 " + lessDeckCount
                        + "장 구성해주세요. (현재 " + PlayerPrefs.GetInt("DeckCount") + "장)";
            }

            // Debug.Log(PlayerPrefs.GetInt("Stage"));
            // Debug.Log(PlayerPrefs.GetInt("ClearStage"));

           


            fadeController.gameObject.SetActive(true);
            fadeController.FadeOut(1f, () => {
                  fadeController.gameObject.SetActive(false);

                  if (stage == PlayerPrefs.GetInt("ClearStage"))
                  {
                        // 처음 깬 스테이지라면.
                        if (stage == 11)
                        {

                        }
                        else
                        {
                              stageButtons[stage].GetComponent<StageButton>().OnMouseUp();
                        }
                  }
            });
      }

      // Update is called once per frame
      void Update()
      {

      }

      public void ChangeSelectedStage(StageButton stageButton)
      {
            // 선택 스테이지 설정.
            selectedStage = stageButton;

            stageNumber.text =  "1 - " + selectedStage.stageNumber;
            se_BossHp = selectedStage.bossHp;
            se_BossDamage = selectedStage.bossDamage;
            se_BossNumber = selectedStage.enemy_number;
      }
      
      public void AllSelectGroupOff()
      {
            for(int i  = 0; i < selectGroups.Length; i++)
            {
                  selectGroups[i].SetActive(false);
            }
      }

      public void StartButtonClick()
      {
            if (PlayerPrefs.GetInt("DeckCount") < lessDeckCount)
            {

            }
            else
            {
                  se_BossHp = selectedStage.bossHp;
                  se_BossDamage = selectedStage.bossDamage;
                  se_BossNumber = selectedStage.enemy_number;

                  PlayerPrefs.SetInt("Stage", selectedStage.stageNumber);
                  PlayerPrefs.SetFloat("BossHP", se_BossHp);
                  PlayerPrefs.SetFloat("BossDmg", se_BossDamage);
                  PlayerPrefs.SetInt("BossNumber", se_BossNumber);

                  fadeController.gameObject.SetActive(true);
                  fadeController.FadeIn(1f, () =>
                  {
                        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");
                  });
            }
      }
      public void CardButtonClick()
      {
            fadeController.gameObject.SetActive(true);
            fadeController.FadeIn(1f, () =>
            {
                  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("CardSelect");
            });
      }
      public void ClearInfoReset()
      {
            Debug.Log("Clear Info Reset ");
            PlayerPrefs.DeleteKey("Stage");
            PlayerPrefs.DeleteKey("ClearStage");
            PlayerPrefs.DeleteKey("tutorial");

            File.Delete(Application.persistentDataPath + "/Data/HasCard.json");
            File.Delete(Application.persistentDataPath + "/Data/CardInfo.json");
            File.Delete(Application.persistentDataPath + "/Data/DeckList.json");

            fadeController.gameObject.SetActive(true);
            fadeController.FadeIn(1f, () =>
            {
                  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Title");
            });
      }
}
