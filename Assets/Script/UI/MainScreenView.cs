using System;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button openButton;

    public void SubscribeOpenButton(Action callback)
    {
        openButton.onClick.AddListener(callback.Invoke);
    }

    public void UnsubscribeOpenButton(Action callback)
    {
        openButton.onClick.RemoveListener(callback.Invoke);
    }
}
