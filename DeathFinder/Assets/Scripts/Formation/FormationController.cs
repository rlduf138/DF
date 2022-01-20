using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using PixelSilo.Helper;

public class FormationController : MonoBehaviour
{
      public FadeController fadeController;
      public FormationJsonParser formationJsonParser;

      [Header("InfoPanel")]
      public GameObject infoPanel;
      public Text name_text;
      public Image demonImage;
      public List<Image> attacks;
      public List<Image> healths;
      public List<Image> ranges;
      public Sprite nullSprite;
      public Sprite attackSprite;
      public Sprite healthSprite;
      public Sprite rangeSprite;
      private Sprite[] animationSprites;

      [Header("FormationButton")]
      public Sprite buttonOriginImage;
      public Button[] currentPositions;
      public Button[] attackPositions;
      public Button[] defencePositions;
      public Button[] utilPositions;
      
      public GameObject[] attackArrows;
      public GameObject[] defenceArrows;
      public GameObject[] utilArrows;


      public int clicked_button_num = -1;

      public DemonButton selectedDB;

      public GameObject attack_panel;
      public GameObject defence_panel;
      public GameObject util_panel;

      public Image attackButton;
      public Image defenceButton;
      public Image utilButton;

      public bool attackFlag;
      public bool defenceFlag;
      public bool utilFlag;

      public List<int> currentList = new List<int>();
      public List<int> attackList;
      public List<int> defenceList;
      public List<int> utilList;

      JsonData saveData;
      List<Information> infos;

      JsonData loadData;
      string jsonString;

      public GameObject closeDoorPanel;

      // Start is called before the first frame update
      void Start()
      {
            // 화면 꺼짐 방지
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            fadeController.gameObject.SetActive(true);
            fadeController.FadeOut(1f, () => {
                  fadeController.gameObject.SetActive(false);
            });

            InitFormation();
      }

      public void InitFormation()
      {
            if (File.Exists(Application.persistentDataPath + "/Data/AttackFormation.json"))
            {
                  AttackFormationButton();
                  jsonString = File.ReadAllText(Application.persistentDataPath + "/Data/AttackFormation.json");
                  loadData = JsonMapper.ToObject(jsonString);

                  for (int i = 0; i < loadData.Count; i++)
                  {
                        clicked_button_num = (int)loadData[i]["position_number"];
                        if ((int)loadData[i]["demon_index"] != -1)
                        {
                              SettingPositionButton(formationJsonParser.demon_button[(int)loadData[i]["demon_index"]].GetComponent<DemonButton>());
                        }
                  }
            }
            else Debug.Log("AttackJson 없음");

            if(File.Exists(Application.persistentDataPath + "/Data/DefenceFormation.json"))
            {
                  DefenceFormationButton();
                  jsonString = File.ReadAllText(Application.persistentDataPath + "/Data/DefenceFormation.json");
                  loadData = JsonMapper.ToObject(jsonString);

                  for (int i = 0; i < loadData.Count; i++)
                  {
                        clicked_button_num = (int)loadData[i]["position_number"];
                        if ((int)loadData[i]["demon_index"] != -1)
                        {
                              SettingPositionButton(formationJsonParser.demon_button[(int)loadData[i]["demon_index"]].GetComponent<DemonButton>());
                        }
                  }
            }
            else Debug.Log("DefenceJson 없음");

            if (File.Exists(Application.persistentDataPath + "/Data/UtilFormation.json"))
            {
                  UtilFormationButton();
                  jsonString = File.ReadAllText(Application.persistentDataPath + "/Data/UtilFormation.json");
                  loadData = JsonMapper.ToObject(jsonString);

                  for (int i = 0; i < loadData.Count; i++)
                  {
                        clicked_button_num = (int)loadData[i]["position_number"];
                        if ((int)loadData[i]["demon_index"] != -1)
                        {
                              SettingPositionButton(formationJsonParser.demon_button[(int)loadData[i]["demon_index"]].GetComponent<DemonButton>());
                        }
                  }
            }
            else Debug.Log("UtilJson 없음");

            if (PlayerPrefs.GetString("formation") == "attack")
            {
                  AttackFormationButton();
            }else if(PlayerPrefs.GetString("formation") == "defence")
            {
                  DefenceFormationButton();
            }else if(PlayerPrefs.GetString("formation") == "util")
            {
                  UtilFormationButton();
            }
      }

      public void DemonCardClick(string _name, Sprite demonSprite, int attackLevel, int healthLevel, int rangeLevel, Sprite[] animation_sprites, DemonButton demonButton)
      {
            // 데몬 카드 클릭시 인포에 띄움
            if(selectedDB != demonButton)
                  selectedDB?.select.SetActive(false);

            StopCoroutine("DemonAnimation");
            infoPanel.SetActive(true);
            name_text.text = _name;
            demonImage.sprite = demonSprite;
            demonImage.SetNativeSize();
            for(int i = 0; i<4; i++)
            {
                  if (i < attackLevel)
                        attacks[i].sprite = attackSprite;
                  else
                        attacks[i].sprite = nullSprite;
            }
            for (int i = 0; i < 4; i++)
            {
                  if (i < healthLevel)
                        healths[i].sprite = healthSprite;
                  else
                        healths[i].sprite = nullSprite;
            }
            for (int i = 0; i < 4; i++)
            {
                  if (i < rangeLevel)
                        ranges[i].sprite = rangeSprite;
                  else
                        ranges[i].sprite = nullSprite;
            }
            animationSprites = animation_sprites;
            StartCoroutine("DemonAnimation");
            selectedDB = demonButton;
      }

      public IEnumerator DemonAnimation()
      {
            int i = 0;
            while(true)
            {
                  if (i == animationSprites.Length) i = 0;

                  demonImage.sprite = animationSprites[i];
                  demonImage.SetNativeSize();
                  yield return new WaitForSeconds(0.15f);
                  i++;
            }
      }


      public void OnPositionButton(int i)
      {
            OffAllArrow();
            clicked_button_num = i;
            if(attackFlag == true)
            {
                  attackArrows[i].SetActive(true);
            }else if(defenceFlag == true)
            {
                  defenceArrows[i].SetActive(true);
            }
            else if(utilFlag == true)
            {
                  utilArrows[i].SetActive(true);
            }
      }

      public void SettingPositionButton(DemonButton demonButton)
      {
            if (attackButton.enabled == true)
            {
                  currentList = attackList;
                  currentPositions = attackPositions;
            }
            else if (defenceButton.enabled == true)
            {
                  currentList = defenceList;
                  currentPositions = defencePositions;
            }
            else if (utilButton.enabled == true)
            {
                  currentList = utilList;
                  currentPositions = utilPositions;
            }

            SettingCurrentPositionButton(demonButton);
      }

      public void SettingCurrentPositionButton(DemonButton demonButton)
      {
            bool check = false;
            if (clicked_button_num != -1)
            {
                  int i = 0;
                  for (i = 0; i < 6; i++)
                  {
                        // 중복 체크.
                        if (currentList[i] == demonButton.demon_index)
                        {
                              // 중복..
                              check = true;
                              break;
                        }
                  }
                  if (check == true)
                  {
                        // 중복된경우. i에 있는 리스트에 중복이 있음.
                        currentPositions[i].image.sprite = buttonOriginImage;
                        currentPositions[i].image.SetNativeSize();
                        currentList.RemoveAt(i);
                        currentList.Insert(i, -1);

                  }

                  currentPositions[clicked_button_num].image.sprite = demonButton.demonSprite;
                  currentPositions[clicked_button_num].image.SetNativeSize();
                  currentList.RemoveAt(clicked_button_num);
                  currentList.Insert(clicked_button_num, demonButton.demon_index);
                  clicked_button_num = -1;
                  OffAllArrow();

                  if (attackButton.enabled == true)
                        attackList = currentList;
                  else if (defenceButton.enabled == true)
                        defenceList = currentList;
                  else if (utilButton.enabled == true)
                        utilList = currentList;
            }
      }
     
      public void OffAllArrow()
      {
            for(int i = 0; i<6; i++)
            {
                  attackArrows[i].SetActive(false);
                  defenceArrows[i].SetActive(false);
                  utilArrows[i].SetActive(false);
            }
      }
            

      public void AttackFormationButton()
      {
            OffAllArrow();
            attack_panel.SetActive(true);
            defence_panel.SetActive(false);
            util_panel.SetActive(false);

            attackButton.enabled = true;
            defenceButton.enabled = false;
            utilButton.enabled = false;

            attackFlag = true;
            defenceFlag = false;
            utilFlag = false;

            clicked_button_num = -1;
      }
      public void DefenceFormationButton()
      {
            OffAllArrow();
            attack_panel.SetActive(false);
            defence_panel.SetActive(true);
            util_panel.SetActive(false);

            attackButton.enabled = false;
            defenceButton.enabled = true;
            utilButton.enabled = false;

            attackFlag = false;
            defenceFlag = true;
            utilFlag = false;

            clicked_button_num = -1;
      }
      public void UtilFormationButton()
      {
            OffAllArrow();
            attack_panel.SetActive(false);
            defence_panel.SetActive(false);
            util_panel.SetActive(true);

            attackButton.enabled = false;
            defenceButton.enabled = false;
            utilButton.enabled = true;

            attackFlag = false;
            defenceFlag = false;
            utilFlag = true;

            clicked_button_num = -1;
      }

      public void SubmitButton()
      {
            if (CheckFormation())
            {
                  SaveData();

                  StartCoroutine("LoadScene");
            }
            
      }
      public bool CheckFormation()
      {
            for(int i = 0; i< 6; i++)
            {
                  if (currentList[i] != -1)
                  {
                        return true;
                  }
            }
            Debug.Log("하나도 선택하지않음");
            return false;
      }

      public IEnumerator LoadScene()
      {
            closeDoorPanel.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");
      }

      public void SaveData()
      {
            infos = new List<Information>();
            for (int i = 0; i < 6; i++)
            {
                  if (attackFlag == true)
                  {
                        Information info = new Information(i, attackList[i]);
                        infos.Add(info);
                  }
                  else if (defenceFlag == true)
                  {
                        Information info = new Information(i, defenceList[i]);
                        infos.Add(info);
                  }
                  else if (utilFlag == true)
                  {
                        Information info = new Information(i, utilList[i]);
                        infos.Add(info);
                  }
            }
            Debug.Log("data : " + infos.ToString());
            saveData = JsonMapper.ToJson(infos);
            if(attackFlag == true)
            {
                  PlayerPrefs.SetString("formation", "attack");
                  File.WriteAllText(Application.persistentDataPath + "/Data/AttackFormation.json", saveData.ToString());
            }else if (defenceFlag == true)
            {
                  PlayerPrefs.SetString("formation", "defence");
                  File.WriteAllText(Application.persistentDataPath + "/Data/DefenceFormation.json", saveData.ToString());
            }else if (utilFlag == true)
            {
                  PlayerPrefs.SetString("formation", "util");
                  
                  File.WriteAllText(Application.persistentDataPath + "/Data/UtilFormation.json", saveData.ToString());
            }

      }
      
}
