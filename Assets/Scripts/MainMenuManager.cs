#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
   public TMP_InputField nameInput;
   public GameObject HighScoreList;
   public GameObject HighScorePrefab;

   // Start is called before the first frame update
   void Start()
   {
      nameInput.text = SaveManager.Instance.Name;
      PopulateLeaderboard(SaveManager.Instance.HighScores);
   }

   void PopulateLeaderboard(List<HighScore> highScoreList)
   {
      if (highScoreList == null || highScoreList.Count == 0)
      {
         HighScoreList.SetActive(false);
         return;
      }

      foreach(HighScore highScore in highScoreList)
      {
         var scoreUIInstance = Instantiate(HighScorePrefab, HighScoreList.transform);
         scoreUIInstance.GetComponent<TextMeshProUGUI>().text = $"{highScore.Name}: {highScore.Score}";
      }
   }

   public void OnStartGame()
   {
      SaveManager.Instance.Name = nameInput.text;
      SceneManager.LoadScene(1);
   }

   public void OnExitGame()
   {
      SaveManager.Instance.SaveHighScores();
#if UNITY_EDITOR
      EditorApplication.ExitPlaymode();
#else
      Application.Quit();
#endif
   }
}
