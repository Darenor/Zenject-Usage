using System;

public class Score
{
    public int CurrentScore { get; private set; }

    public event Action<int> OnScoreChanged;

    public void AddPoint()
    {
        CurrentScore++;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void SetScore(int value)
    {
        CurrentScore = value;
        OnScoreChanged?.Invoke(CurrentScore);
    }
}
