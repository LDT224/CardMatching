using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "gameSaveData.json");
    }

    [System.Serializable]
    public class GameData
    {
        public int score;
        public int combo;
        public int compareRight;
        public int mode;
        public List<CardData> cards = new List<CardData>();
    }

    public void SaveGame(int score, int combo, int compareRight, int mode, List<GameObject> cards)
    {
        GameData gameData = new GameData
        {
            score = score,
            combo = combo,
            compareRight = compareRight,
            mode = mode
        };

        for (int i = 0; i < cards.Count; i++)
        {
            gameData.cards.Add(new CardData(cards[i], i));
        }

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Game Saved!");
    }

    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded!");
            return gameData;
        }
        else
        {
            Debug.LogWarning("No saved game found!");
            return null;
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted!");
        }
        else
        {
            Debug.LogWarning("No save file to delete!");
        }
    }
}
