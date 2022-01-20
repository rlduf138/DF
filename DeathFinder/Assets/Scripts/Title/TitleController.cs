using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class TitleController : MonoBehaviour
{
      private const string Scene = "test_map";

      public FadeController fader;
      public Image circle_effect_image;
      public AudioSource audioSource;

      public Image backImage;
      public Image textImage;

      public Sprite[] backgroundImgs;
      public Sprite[] textImgs;
      public Text loadingText;

      public bool first_flag;
      public bool loading_flag;


      private void Start()
      {

            //    StartCoroutine(CircleEffect());
            Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;

            //fader.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
            fader.InitColor(new Color(0f, 0f, 0f, 1f));
            fader.FadeOut(1f, () =>
            {
                  //    audioSource.Play();
                  StartCoroutine("TitleAnimation");
            });

            // 월드맵으로 옮겨야함.
            //PlayerPrefs.DeleteKey("Stage");

            StartCoroutine("CheckData");

            if (!PlayerPrefs.HasKey("Stage"))
            {
                  Debug.Log("Generate Prefs Key - Stage");
                  PlayerPrefs.SetInt("Stage", 1);
            }
            if (!PlayerPrefs.HasKey("ClearStage"))
            {
                  // 클리어 스테이지 정보가 없으면.
                  Debug.Log("Generate Prefs Key - ClearStage");
                  PlayerPrefs.SetInt("ClearStage", 0);
            }
      }

      public IEnumerator CheckData()
      {
            Debug.Log("CheckData()");
            loading_flag = true;
            loadingText.enabled = true;
            if(!Directory.Exists(Application.persistentDataPath + "/Data"))
            {
                  Directory.CreateDirectory(Application.persistentDataPath + "/Data");
            }
            if (!File.Exists(Application.persistentDataPath + "/Data/HasCard.json"))
            {
                  // 첫 실행이다.
                  first_flag = true;
                  PlayerPrefs.SetFloat("tutorial", 1);
            }
            else
                  PlayerPrefs.SetFloat("tutorial", 0);

            CopyData();

            loading_flag = false;
            loadingText.enabled = false;
            yield return null;
      }

      public void CopyData()
      {
            Debug.Log("CopyData");
            string srcPath = Application.streamingAssetsPath + "/DB/CardInfo.json";
            string destPath = Application.persistentDataPath + "/Data/CardInfo.json";
           // if (!File.Exists(Application.persistentDataPath + "/Data/CardInfo.json"))
          //  {
                  using (WWW request = new WWW(srcPath))
                  {

                        while (!request.isDone) {; }

                        if (!string.IsNullOrEmpty(request.error))
                        {
                              Debug.LogWarning(request.error);

                              return;
                        }

                        File.WriteAllBytes(destPath, request.bytes);
                  }
            //   }

            string srcPath2 = Application.streamingAssetsPath + "/DB/StartingDeck.json";
            string destPath2 = Application.persistentDataPath + "/Data/StartingDeck.json";
            // if (!File.Exists(Application.persistentDataPath + "/Data/CardInfo.json"))
            //  {
            using (WWW request = new WWW(srcPath2))
            {

                  while (!request.isDone) {; }

                  if (!string.IsNullOrEmpty(request.error))
                  {
                        Debug.LogWarning(request.error);

                        return;
                  }

                  File.WriteAllBytes(destPath2, request.bytes);
            }
            //   }
      }

      /*     public void TestCode()
           {

                 string DATZIP_PATH = Application.streamingAssetsPath + "/" + "Test1" + "/";

                 string DAT_PATHTMP = Application.persistentDataPath + "/" + "Test1" + "/tmp/";

                 string zipDatFilePath = DATZIP_PATH + "file.zip";
                 string cpzipDatFilePath = DAT_PATHTMP + "file.zip";



                 StartCoroutine(DownloadFile(zipDatFilePath, cpzipDatFilePath,
                    (object obj) =>
                    {

                    }));

           }



           public static IEnumerator DownloadFile(string filePath, string savePath, DGEventHandler fEvt = null)
           {
                 ///////////////////////////////////////////////////////////////////////////////////// 
                 ///////////////////////////////////////////////////////////////////////////////////// 
                 //{IOS Path Add "file://" 

#if UNITY_IOS //IOS 인경우"file://"이 들어가야 읽어줄수 있다. 
       filePath = "file://"+filePath; 
#endif
                 //} 
                 ///////////////////////////////////////////////////////////////////////////////////// 
                 ///////////////////////////////////////////////////////////////////////////////////// 

                 UnityWebRequest uwr = new UnityWebRequest(filePath);

                 uwr.downloadHandler = new DownloadHandlerFile(savePath);
                 yield return uwr.SendWebRequest();

                 if (fEvt != null)
                 {
                       fEvt(null);
                 }
           }

     */


      public IEnumerator TitleAnimation()
      {
            int i = 0;
            while (true)
            {
                  if (i == backgroundImgs.Length)
                  {
                        i = 0;
                  }

                  yield return new WaitForSeconds(0.15f);

                  backImage.sprite = backgroundImgs[i];
                  textImage.sprite = textImgs[i];

                  i++;
            }
      }

      // Update is called once per frame
      public void GameStart()
      {
            //AudioSource audioSource = GetComponent<AudioSource>();
            //  audioSource.Play();
            StartCoroutine(ActiveScene());
      }

      public void OnMouseDown()
      {
            if (loading_flag == false)
            {
                  Debug.Log("OnMouseUp");
                  GameStart();
            }
      }



      IEnumerator ActiveScene()
      {
            //   fader.FadeOut(0.7f);
            //  yield return new WaitForSeconds(2f);
            fader.FadeIn(1f, () =>
            {
                  /*  if (first_flag == true)
                    {
                          UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StartingDeck");
                    }
                    else if (first_flag == false)
                    {
                          UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StageSelect");
                    }*/
                  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("New Scene");
            });
            yield break;
      }
}
