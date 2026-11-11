using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelView : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button collectButton;

    [SerializeField] private Image panelImage;
    [SerializeField] private TMP_Text scoreText;

    public Image PanelImage => panelImage;

    public void SubscribeCloseButton(Action callback)
    {
        closeButton.onClick.AddListener(callback.Invoke);
    }

    public void UnsubscribeCloseButton(Action callback)
    {
        closeButton.onClick.RemoveListener(callback.Invoke);
    }

    public void SubscribeCollectButton(Action callback)
    {
        collectButton.onClick.AddListener(callback.Invoke);
    }

    public void UnsubscribeCollectButton(Action callback)
    {
        collectButton.onClick.RemoveListener(callback.Invoke);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
