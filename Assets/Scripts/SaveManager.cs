using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
class SaveData
{
   public List<HighScore> HighScores;
}

[Serializable]
public class HighScore
{
   public string Name;
   public int Score;
}

public class SaveManager : MonoBehaviour
{
   public static SaveManager Instance;

   public int maxHighScores = 5;
   public List<HighScore> HighScores;
   public string Name;

   private string savePath;

   private void Awake()
   {
      if (Instance != null)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
      savePath = Application.persistentDataPath + "/savedata.json";
      DontDestroyOnLoad(gameObject);
      LoadHighScores();
   }

   public void AddScore(string name, int score)
   {
      if(HighScores == null)
         HighScores = new List<HighScore>();

      int index = 0;
      for(; index < HighScores.Count; index++)
      {
         if (score > HighScores[index].Score)
            break;
      }

      if(index < maxHighScores)
      {
         HighScores.Insert(index, new HighScore() { Name = name, Score = score });
      }

      if (HighScores.Count > maxHighScores)
         HighScores.RemoveAt(maxHighScores);
   }

   public void SaveHighScores()
   {
      SaveData saveData = new SaveData();
      saveData.HighScores = new List<HighScore>();
      saveData.HighScores.AddRange(HighScores);

      string json = JsonUtility.ToJson(saveData);
      File.WriteAllText(savePath, json);
   }

   public void LoadHighScores()
   {
      if (File.Exists(savePath))
      {
         var saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
         foreach (var score in saveData.HighScores)
            AddScore(score.Name, score.Score);
      }
   }
}
