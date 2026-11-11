using UnityEngine;
using Zenject;

public class PanelController : IUIState
{
    private readonly PanelView view;
    private readonly Score score;

    private readonly IFadeService fadeService;
    private readonly ISoundPlayerService soundPlayer;
    private readonly ISaverService saver;

    public event System.Action ClosePanelRequested;

    [Inject]
    public PanelController(
        PanelView view,
        Score score,
        IFadeService fadeService,
        ISoundPlayerService soundPlayer,
        ISaverService saver)
    {
        this.view = view;
        this.score = score;
        this.fadeService = fadeService;
        this.soundPlayer = soundPlayer;
        this.saver = saver;
    }

    public void Initialize()
    {
        view.gameObject.SetActive(false);
    }

    public void Enter()
    {
        view.gameObject.SetActive(true);

        view.SubscribeCloseButton(ClosePanel);
        view.SubscribeCollectButton(CollectPoint);

        score.OnScoreChanged += view.UpdateScore;

        view.UpdateScore(score.CurrentScore);

        fadeService.FadeIn(view.PanelImage, 0.4f);
        soundPlayer.PlayOpenSound();
    }

    public void Exit()
    {
        view.UnsubscribeCloseButton(ClosePanel);
        view.UnsubscribeCollectButton(CollectPoint);

        score.OnScoreChanged -= view.UpdateScore;

        saver.SaveScore();

        fadeService.FadeOut(view.PanelImage, 0.4f);
        soundPlayer.PlayCloseSound();
    }

    private void CollectPoint()
    {
        score.AddPoint();
    }

    private void ClosePanel()
    {
        ClosePanelRequested?.Invoke();
    }
}
