using System;
using System.IO;
using UnityEngine;
using Zenject;

public class JsonSaverService : ISaverService
{
    private readonly Score score;

    private readonly string defaultPath;

    [Inject]
    public JsonSaverService(Score score)
    {
        this.score = score;

        defaultPath = Path.Combine(
            Application.persistentDataPath,
            "score.json"
            );
    }

    [Serializable]
    private class ScoreData
    {
        public int score;
    }

    public void SaveScore(string path = null)
    {
        string savePath = string.IsNullOrWhiteSpace(path)
            ? defaultPath
            : path;

        ScoreData data = new ScoreData
        {
            score = score.CurrentScore
        };

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log($"JSON: сохранено {score.CurrentScore} очков");
    }

    public void LoadScore(string path = null)
    {
        string loadPath = string.IsNullOrWhiteSpace(path)
            ? defaultPath
            : path;

        if (!File.Exists(loadPath))
        {
            Debug.Log("JSON сохранение не найдено");
            return;
        }

        string json = File.ReadAllText(loadPath);

        ScoreData data = JsonUtility.FromJson<ScoreData>(json);

        if (data == null)
            return;

        score.SetScore(data.score);

        Debug.Log($"JSON: загружено {score.CurrentScore} очков");
    }
}
