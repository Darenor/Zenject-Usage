using Zenject;

public class UISwitcher
{
    private IUIState currentState;

    private readonly MainScreenController mainController;
    private readonly PanelController panelController;

    [Inject]
    public UISwitcher(
        MainScreenController mainController,
        PanelController panelController,
        ISaverService saver)
    {
        this.mainController = mainController;
        this.panelController = panelController;

        saver.LoadScore();

        mainController.OpenPanelRequested += SwitchToPanel;
        panelController.ClosePanelRequested += SwitchToMain;

        panelController.Initialize();

        SwitchToMain();
    }

    public void SwitchToMain()
    {
        currentState?.Exit();

        currentState = mainController;
        currentState.Enter();
    }

    public void SwitchToPanel()
    {
        currentState?.Exit();

        currentState = panelController;
        currentState.Enter();
    }
}
