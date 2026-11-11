using UnityEngine;
using Zenject;

public class PlayerPrefsSaver : ISaverService
{
    private readonly Score score;

    [Inject]
    public PlayerPrefsSaver(Score score)
    {
        this.score = score;
    }

    public void SaveScore(string path = null)
    {
        PlayerPrefs.SetInt("PlayerScore", score.CurrentScore);
        PlayerPrefs.Save();

        Debug.Log($"PlayerPrefs: сохранено {score.CurrentScore} очков");
    }

    public void LoadScore(string path = null)
    {
        int savedScore = PlayerPrefs.GetInt("PlayerScore", 0);

        score.SetScore(savedScore);

        Debug.Log($"PlayerPrefs: загружено {savedScore} очков");
    }
}
