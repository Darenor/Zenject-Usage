using System;
using Zenject;

public class MainScreenController : IUIState
{
    private readonly MainScreenView view;

    public event Action OpenPanelRequested;

    [Inject]
    public MainScreenController(MainScreenView view)
    {
        this.view = view;
    }

    public void Enter()
    {
        view.SubscribeOpenButton(OpenPanel);
    }

    public void Exit()
    {
        view.UnsubscribeOpenButton(OpenPanel);
    }

    private void OpenPanel()
    {
        OpenPanelRequested?.Invoke();
    }
}
