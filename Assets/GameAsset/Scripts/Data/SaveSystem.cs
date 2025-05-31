using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved: " + savePath);
    }

    public static GameData Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found.");
            return null;
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<GameData>(json);
    }
    public static bool Exists()
    {
        return File.Exists(Path.Combine(Application.persistentDataPath, "save.json"));
    }
    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted.");
        }
    }
}
