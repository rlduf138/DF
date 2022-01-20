using PixelSilo.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameController : Singleton<GameController>
{
      public BattleData data;
      public GameObject spawnCircle;
      public int level { get; protected set; }
      public GameJsonParser jsonParser;
      //   public GameObject doorPanel;

      public GameObject areaPanel;

      [Header("GameOver")]
      public GameObject gameOverCanvas;
      public FadeController panelFade;
      public FadeController winFade;
      public FadeController loseFade;
      public Image winImage;
      public Image loseImage;
      
      [Header("LeftTop")]
      public GameObject leftTopPanel;
      public Image[] portraitImgs;
      public Sprite noneImage;
      public Slider[] demonSliders;
      public Slider[] skillSliders;
      public Text[] demonNameText;

      [Header("Boss")]
      public Transform bossPosition;
      public GameObject boss01;
      public GameObject[] boss_list;

      [Header("BossHP")]
      public BossBase bossBase;
      public GameObject bossInfoPanel;
      public Text bossNameText;
      public float boss1_maxHp;
      private float boss1_currentHp;
      public Slider boss1_slider;
      public Text bossHpText;

      [Header("DemonList")]
      public List<GameObject> p1_characters;
      public List<bool> p1_dead;
      public List<GameObject> p2_characters;

      [Header("Formation")]
      public GameObject attackFormation;
      public GameObject defenceFormation;
      public GameObject utilFormation;
      public Transform[] formationTransforms;
      public GameObject group;
      public Transform group_front;
      public Transform group_mid;
      public Transform group_back;
      public enum GroupPosition
      {
            Front, Mid, Back
      };
      public GroupPosition groupPosition;

     [Header("SpawnPosition")]
      public Transform p1_spawn;
      public Transform p2_spawn;

      [Header("FirstPosition")]
      public List<GameObject> p1_firstpos;
      public List<GameObject> p2_firstpos;

     
      //[Header("DemonList")]
      //public List<GameObject> demonList;
      [Header("Minion")]
      public GameObject minion;

      [Header("Revival")]
      public GameObject revivalEffect;

      // Start is called before the first frame update
      void Start()
      {
            Debug.Log("GameStart");
            level = 1;
            leftTopPanel.SetActive(false);
            ManaController.Instance.gameObject.SetActive(false);
            CardController.Instance.gameObject.SetActive(false);
            CardController.Instance.cancel_panel.SetActive(true);
            CardController.Instance.deckButton.gameObject.SetActive(false);
            areaPanel.SetActive(false);
            bossInfoPanel.SetActive(false);

            //  boss1_slider.maxValue = boss1_maxHp;
            //   boss1_currentHp = boss1_maxHp;
            //  Boss1Refresh(0);

            InitFormation();
            groupPosition =  GroupPosition.Mid;
            // Mulligan();
            //   GameStart();
      }

      public void InitBoss(BossBase bB)
      {
            bossBase = bB;

            bossBase.hpBar = boss1_slider;
            bossBase.hpBar.maxValue = bossBase.startingHealth;
            bossBase.hpBar.value = bossBase.health;
            bossBase.bossHpText = bossHpText;
            //bossNameText.text = bossBase.boss_name;
            bossNameText.text = "1 - " + PlayerPrefs.GetInt("Stage");
      }

      public void InitFormation()
      {
            /* if (PlayerPrefs.GetString("formation") == "attack")
             {
                   formationTransforms = attackFormation.GetComponentsInChildren<Transform>();
             }
             else if (PlayerPrefs.GetString("formation") == "defence")
             {
                   formationTransforms = defenceFormation.GetComponentsInChildren<Transform>();
             }
             else if (PlayerPrefs.GetString("formation") == "util")
             {
                   formationTransforms = utilFormation.GetComponentsInChildren<Transform>();
             }
             */
            formationTransforms = attackFormation.GetComponentsInChildren<Transform>();

            for (int i = 0; i < 6; i++)
            {
                  formationTransforms[i] = formationTransforms[i + 1];
            }
            formationTransforms[6] = null;
      }



      // 02. GameStart
      public void GameStart()
      {
            Debug.Log("GameStart");


            spawnCircle.SetActive(true);

            // 보스 소환.
            int bossNumber = PlayerPrefs.GetInt("BossNumber");
            GameObject boss = Instantiate(boss_list[bossNumber], bossPosition.position, Quaternion.identity);

            boss.GetComponent<BossBase>().health = boss.GetComponent<BossBase>().startingHealth
                  = PlayerPrefs.GetFloat("BossHP");
            boss.GetComponent<BossBase>().damage = PlayerPrefs.GetFloat("BossDmg");
            p2_characters.Add(boss);
            //        boss.GetComponent<BossBase>().InitBoss();
      }

      /*public GameObject SpawnWithParser(int n)
      {
            GameObject gameObject = Instantiate(demonList[jsonParser.infos[n].demon_index], formationTransforms[n].position, Quaternion.identity);
            DemonBase db = gameObject.GetComponent<DemonBase>();
            portraitImgs[n].sprite = db.portrait;
            db.hpBar = demonSliders[n];
            db.hpBar.maxValue = db.startingHealth;
            db.skillSlider = skillSliders[n];
            db.HpBarRefresh(db.health);
            demonNameText[n].text = db.demon_name;

            p1_dead[n] = false;

            gameObject.tag = "Player";
            gameObject.layer = LayerMask.NameToLayer("Player");
            gameObject.GetComponent<DemonBase>().Flip(1);
         //   gameObject.GetComponent<DemonBase>().index = p1_characters.IndexOf(gameObject);

            return gameObject;
            
      }*/

      public GameObject SpawnMinion(int n)
      {
            GameObject gameObject = Instantiate(minion, formationTransforms[n].position, Quaternion.identity);

            p1_dead[n] = false;

            return gameObject;
      }

      public IEnumerator SpawnWithFormation()
      {
            // 6마리 순차적으로 소환.

            //yield return new WaitForSeconds(3f);

            // leftTopPanel.SetActive(true);

            for (int i = 0; i < 6; i++)
            {

                  GameObject gameObject = SpawnMinion(i);
                  gameObject.GetComponent<MinionBase>().myPosition = group.GetComponent<Group>().positions[i];

                  p1_characters.Add(gameObject);

                  gameObject.GetComponent<MinionBase>().index = p1_characters.IndexOf(gameObject);

                  yield return new WaitForSeconds(0.1f);


            }
            for (int i = 0; i < p1_characters.Count; i++)
            {
                  if (p1_characters[i] != null)
                  {
                        p1_characters[i].GetComponent<MinionBase>().stop = false;
                  }
            }

            CardController.Instance.gameObject.SetActive(true);
            CardController.Instance.ShuffleAllCard();
            ManaController.Instance.gameObject.SetActive(true);
            ManaController.Instance.Starting();
            CardController.Instance.deckButton.gameObject.SetActive(true);
            areaPanel.SetActive(true);
            bossInfoPanel.SetActive(true);

            ManaController.Instance.StartCoroutine("ManaRestore");
       
            yield return new WaitForSeconds(0.4f);

            spawnCircle.SetActive(false);
            CardController.Instance.cancel_panel.SetActive(false);
            p2_characters[0].GetComponent<BossBase>().stop = false;
      }

      public IEnumerator GameOver()
      {
          
            gameOverCanvas.SetActive(true);

            panelFade.FadeIn(0.9f, () =>
            {
                 
            });
            loseImage.enabled = true;
            loseFade.FadeIn(1f, () =>
            {
                  Debug.Log("StopAllCoroutine");
                  StopAllCoroutines();

                  Time.timeScale = 0f;
            });

            yield return null;
      }

      public IEnumerator GameWin()
      {
            int stage = PlayerPrefs.GetInt("Stage");

            // 클리어스테이지와 현재스테이지 비교해서 다시 꺤건지 처음 꺤건지.
            if (stage > PlayerPrefs.GetInt("ClearStage"))
            {
                  // 처음 깬 스테이지라면.
                  PlayerPrefs.SetInt("ClearStage", stage);
            }
            //    stage++;
            //    if (stage == 11)
            //     {
            //          stage = 10;
            //   }
            //   PlayerPrefs.SetInt("Stage", stage);

            gameOverCanvas.SetActive(true);

            panelFade.FadeIn(0.9f, () =>
            {

            });
            winImage.enabled = true;
            winFade.FadeIn(1f, () =>
            {
                  Debug.Log("StopAllCoroutine");
                  StopAllCoroutines();

                  Time.timeScale = 0f;
            });

            yield return null;
      }

      public void OnRestartClick()
      {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StageSelect");
      }

      //======================== MagicCard ==============================

      public void ManaEater(int min, int max)
      {
            int ran = Random.Range(min, max + 1);
            ManaController.Instance.RestoreMana(ran);
      }
      public IEnumerator DoubleCard()
      {
            // CardController.Instance._sorting = false;
            //  CardController.Instance.StartCoroutine("AddCard");
            //  CardController.Instance._adding = true;
            //      yield return new WaitForSeconds(0.2f);
            //   CardController.Instance.StartCoroutine("AddCard");
            //   CardController.Instance._sorting = true;

            CardController.Instance.StartCoroutine("DoubleCardStart");
             
            yield return null;
      }
      public void WeaknessCard(float percent)
      {
            bossBase.WeaknessOn(percent);
      }
      public void WeaknessCardEnd()
      {
            bossBase.WeaknessOff();
      }
      public void FreezeCardOn(float percent)
      {
            bossBase.FreezeOn(percent);
      }
      public void FreezeCardEnd()
      {
            bossBase.FreezeOff();
      }
      public void CondemnCardOn(float percent)
      {
            bossBase.CondemnOn(percent);
      }
      public void CondemnCardOff()
      {
            bossBase.CondemnOff();
      }
     

      public void ManaBoomCard(int min, int max)
      {
            int ran = Random.Range(min, max + 1);
            bossBase.StartCoroutine("ManaBoom", ran);

      }
      public void HealCard(float percent)
      {
            for (int i = 0; i < p1_characters.Count; i++)
            {
                  if (p1_dead[i] == false)
                  {
                        if (p1_characters[i] != null)
                        {
                              MinionBase db = p1_characters[i].GetComponent<MinionBase>();
                              db.StartCoroutine("HealEffectOn");
                              db.RestoreHealth(db.startingHealth * percent/100);

                        }
                  }
            }
      }
      public void BerserkerCard()
      {
            for (int i = 0; i < p1_characters.Count; i++)
            {
                  if (p1_dead[i] == false)
                  {
                        if (p1_characters[i] != null)
                        {
                              MinionBase db = p1_characters[i].GetComponent<MinionBase>();
                              db.BerserkerOn();
                        }
                  }
            }
      }
      public void BerserkerCardEnd()
      {
            for (int i = 0; i < p1_characters.Count; i++)
            {
                  if (p1_characters[i] != null)
                  {
                        MinionBase db = p1_characters[i].GetComponent<MinionBase>();
                        db.BerserkerOff();
                  }
            }
      }
      public void BarrierCard()
      {
            for (int i = 0; i < 6; i++)
            {
                  if (p1_dead[i] == false)
                  {
                        if (p1_characters[i] != null)
                        {
                              p1_characters[i].GetComponent<MinionBase>().BarrierOn();
                        }
                  }
            }
      }
      public void BarrierCardEnd()
      {
            for (int i = 0; i < 6; i++)
            {
                  if (p1_characters[i] != null)
                  {
                        p1_characters[i].GetComponent<MinionBase>().BarrierOff();
                  }
            }
      }
      public void PoisonCard(float poisonDmg, float time)
      {
            bossBase.poisonDamage = poisonDmg;
            bossBase.poisonTime = time;
            bossBase.StartCoroutine("PoisonMist");
      }
      public IEnumerator RevivalCard()
      {
            List<int> lists = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                  // i = 0~6
                  if (p1_dead[i] == true)
                  {
                        lists.Add(i);

                  }
            }
            if (lists.Count != 0)
            {
                  int ran = Random.Range(0, lists.Count);
                  Debug.Log("ranom : " + ran);

                  //      GameObject revivalObject = Instantiate(revivalEffect, formationTransforms[lists[ran]].position, Quaternion.identity);

                  // yield return new WaitForSeconds(0.8f);

                  // GameObject gameObject = SpawnWithParser(lists[ran]);

                  // p1_characters.RemoveAt(lists[ran]);
                  // p1_characters.Insert(lists[ran], gameObject);

                  //  gameObject.GetComponent<DemonBase>().index = p1_characters.IndexOf(gameObject);

                  // p1_characters[lists[ran]].GetComponent<DemonBase>().stop = false;

                  //yield return new WaitForSeconds(0.9f);

                  //     Destroy(revivalObject);

                  p1_characters[lists[ran]].GetComponent<MinionBase>().StartCoroutine("Revive");

                  yield return null;
            }
      }
      public bool RevivalCheck()
      {
            // 부활 가능한 데몬 있는지 체크.
            List<int> lists = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                  if (p1_dead[i] == true)
                  {
                        return true;
                  }
            }
            return false;
      }

      public void Sacrifice(float percent)
      {
            Debug.Log("Sacrifice()");
            // 남은 미니언이 2마리 이상이어야함.
            int check = 0;
            for(int i = 0; i < p1_dead.Count; i++)
            {
                  if(p1_dead[i] == false) { check++; }
            }
            if(check < 2)
            {
                  // 사용조건에 안맞음.
                  Debug.Log("아군 부족");
                  return;
            }

            // 체력 순으로 정렬.
            GameObject tempObject = p1_characters[0];
            for(int i = 0; i<5; i++)
            {
                  if(tempObject.GetComponent<MinionBase>().health < p1_characters[i + 1].GetComponent<MinionBase>().health)
                  {
                        tempObject = p1_characters[i + 1];
                  }
            }
            // Sacrifice
            float health = tempObject.GetComponent<MinionBase>().health;

            tempObject.GetComponent<MinionBase>().StartCoroutine("Sacrifice");
            Debug.Log("Sacrifice : " + tempObject.GetComponent<MinionBase>().index);

            // heal
            for (int i = 0; i < p1_characters.Count; i++)
            {
                  if (p1_dead[i] == false)
                  {
                        if (p1_characters[i] != null)
                        {
                              MinionBase db = p1_characters[i].GetComponent<MinionBase>();
                              db.StartCoroutine("HealEffectOn");
                              db.RestoreHealth(health * percent / 100);
                              Debug.Log("Sacri Heal : " + health * percent / 100);
                        }
                  }
            }
      }

      //==============================================================//

     

      public void DeleteCharacter(int index)
      {
            p1_dead[index] = true;
            //p1_characters.RemoveAt(index);
            //p1_characters.Insert(index, null);
            GameOverCheck();
      }
      public void GameOverCheck()
      {
            for (int i = 0; i < 6; i++)
            {
                  if (p1_dead[i] == false)
                  {
                        return;
                  }
            }
            Debug.Log("패배");
            StartCoroutine("GameOver");
      }
      public void DeleteCharacter2(int index)
      {
            p2_characters.RemoveAt(index);

            for (int i = 0; i < p2_characters.Count; i++)
            {
                  p2_characters[i].GetComponent<DemonBase>().index = i;
            }
      }

      // ====================Magic  Order ==========================

      public void OrderCharge()
      {
            /*Debug.Log("Magic OrderCharge");
            if(groupPosition == GroupPosition.Front)
            {
                  Debug.Log("Current : Front, After : Front");
            }else if(groupPosition == GroupPosition.Mid)
            {
                  group.transform.position = group_front.position;
                  groupPosition = GroupPosition.Front;
                  Debug.Log("Current : Mid, After : Front");
            }
            else if(groupPosition == GroupPosition.Back)
            {
                  group.transform.position = group_mid.position;
                  groupPosition = GroupPosition.Mid;
                  Debug.Log("Current : Back, After : Mid");
            }
            */
      }
      public void OrderMoveOff()
      {
       /*     Debug.Log("Magic OrderMoveOff");
            if (groupPosition == GroupPosition.Front)
            {
                  group.transform.position = group_mid.position;
                  groupPosition = GroupPosition.Mid;
                  Debug.Log("Current : Front, After : Mid");
            }
            else if (groupPosition == GroupPosition.Mid)
            {
                  group.transform.position = group_back.position;
                  groupPosition = GroupPosition.Back;
                  Debug.Log("Current : Mid, After : Back");
            }
            else if (groupPosition == GroupPosition.Back)
            {
                  Debug.Log("Current : Back, After : Mid");
            }*/
      }
      public void OrderAssemble()
      {
            for(int i = 0; i<p1_characters.Count; i++)
            {
                  p1_characters[i].GetComponent<MinionBase>().assemble = true;
                  CardController.Instance.assembleFlag = true;
            }
      }
      public void OrderReattack()
      {
            for (int i = 0; i < p1_characters.Count; i++)
            {
                  p1_characters[i].GetComponent<MinionBase>().assemble = false;
            }
      }
      
      // ====================== TP System ========================
      /* public void BigWaveEnd()
       {

             tpPanel.SetActive(true);

             if (lv_manaEater == 3) bt_manaEater.enabled = false;
             if (lv_fastDraw == 3) bt_fastDraw.enabled = false;
             if (lv_highRank == 3) bt_highRank.enabled = false;

             Time.timeScale = 0;
       }*/

      /*   public void ManaEaterClick()
         {
               tpPanel.SetActive(false);
               lv_manaEater++;

               // 특성 적용
               if (lv_manaEater == 1 || lv_manaEater == 2)
                     ManaController.Instance.mana_restore_time -= 1f;
               else if (lv_manaEater == 3)
                     ManaController.Instance.mana_restore_time -= 2f;

               Time.timeScale = 1f;
         }
         public void FastDrawClick()
         {
               tpPanel.SetActive(false);
               lv_fastDraw++;

               //특성 적용
               if (lv_fastDraw == 1 || lv_fastDraw == 2)
                     CardController.Instance.cardAddTime -= 1f;
               else if (lv_fastDraw == 3)
                     CardController.Instance.cardAddTime -= 3f;

               Time.timeScale = 1f;
         }*/
      /*public void HighRankSummonClick()
      {
            tpPanel.SetActive(false);
            lv_highRank++;

            // 현재 소환되어있는 데몬의 능력치 변경
            if (lv_highRank == 1)
            {
                  // 1랭크 체력 10프로 증가.
                  for (int i = 0; i < p1_characters.Count; i++)
                  {
                        DemonBase db = p1_characters[i].GetComponent<DemonBase>();
                        db.ChangeHp(db.startingHealth * 0.1f);
                        db.HpBarRefresh(db.health);
                  }
            }
            else if (lv_highRank == 2)
            {
                  // 2랭크 데미지 10프로 증가.
                  for (int i = 0; i < p1_characters.Count; i++)
                  {
                        DemonBase db = p1_characters[i].GetComponent<DemonBase>();
                        db.ChangeDamage(db.damage * 0.1f);
                  }
            }

            // 현재 카드의 능력 변경.
            for (int i = 0; i < 8; i++)
            {
                  if (CardController.Instance.card_enable[i] == true)
                  {
                        CardController.Instance.cards[i].GetComponent<Card>().ChangeCardInfo();
                  }

            }

            Time.timeScale = 1f;
      }*/

      // 01. Mulligan System -> MulliganController에서 GameStart명령.
      public void Mulligan()
      {
            //mulliganCanvas.SetActive(true);
          //  MulliganShuffle();
      }

      public void MulliganShuffle()
      {
         //   MulliganController.Instance.MulliganSetting();
      }

}
