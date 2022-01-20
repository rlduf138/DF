using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OptionController : MonoBehaviour
{
      public GameObject optionPanel;
      public FadeController fadeController;

      public GameObject exitCheck;
      public GameObject dataCheck;

      public void OnOptionClick()
      {
            Time.timeScale = 0f;
            optionPanel.SetActive(true);
      }
      public void OnResumeClick()
      {
            Time.timeScale = 1f;
            optionPanel.SetActive(false);
      }
      public void OnPanelClick()
      {
            Time.timeScale = 1f;
            optionPanel.SetActive(false);
      }
      public void OnSetClick()
      {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StageSelect");
      }

      public void OnExitCheck()
      {
            exitCheck.SetActive(true);
      }
      public void OnDataCheck()
      {
            dataCheck.SetActive(true);
      }
      public void CheckCancel()
      {
            exitCheck.SetActive(false);
            dataCheck.SetActive(false);
      }

      public void OnExitClick()
      {
            Application.Quit();
      }

      public void DataReset()
      {
            Debug.Log("Clear Info Reset Start ");
            PlayerPrefs.DeleteKey("Stage");
            PlayerPrefs.DeleteKey("ClearStage");
            PlayerPrefs.DeleteKey("tutorial");

            File.Delete(Application.persistentDataPath + "/Data/HasCard.json");
            File.Delete(Application.persistentDataPath + "/Data/CardInfo.json");
            File.Delete(Application.persistentDataPath + "/Data/DeckList.json");
            Debug.Log("ClearInfo End ->  FadeIn");
            fadeController.gameObject.SetActive(true);
            Time.timeScale = 1f;
            fadeController.FadeIn(1f, () =>
            {
                  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Title");
                  
            });
      }
      // ======================== 더미 =========================
      public void OnClearClick()
      {
            Time.timeScale = 1f;
            optionPanel.SetActive(false);
            GameController.Instance.StartCoroutine("GameWin");
      }
}
