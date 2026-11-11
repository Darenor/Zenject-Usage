using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [Header("UI")]
    [SerializeField] private MainScreenView mainScreenView;
    [SerializeField] private PanelView panelView;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    [Header("Save Settings")]
    [SerializeField] private SaveModeService saveMode;

    public override void InstallBindings()
    {
        Container.Bind<MainScreenView>()
            .FromInstance(mainScreenView)
            .AsSingle();

        Container.Bind<PanelView>()
            .FromInstance(panelView)
            .AsSingle();

        Container.Bind<AudioSource>()
            .FromInstance(audioSource)
            .AsSingle();

        Container.Bind<AudioClip>()
            .WithId("OpenClip")
            .FromInstance(openClip);

        Container.Bind<AudioClip>()
            .WithId("CloseClip")
            .FromInstance(closeClip);

        Container.Bind<Score>()
            .AsSingle()
            .NonLazy();

        Container.Bind<IFadeService>()
            .To<FadeService>()
            .AsSingle()
            .NonLazy();

        Container.Bind<ISoundPlayerService>()
            .To<SoundPlayerService>()
            .AsSingle()
            .NonLazy();

        switch (saveMode)
        {
            case SaveModeService.Json:
                Container.Bind<ISaverService>()
                    .To<JsonSaverService>()
                    .AsSingle()
                    .NonLazy();
                break;

            case SaveModeService.PlayerPrefs:
                Container.Bind<ISaverService>()
                    .To<PlayerPrefsSaver>()
                    .AsSingle()
                    .NonLazy();
                break;
        }

        Container.Bind<MainScreenController>()
            .AsSingle()
            .NonLazy();

        Container.Bind<PanelController>()
            .AsSingle()
            .NonLazy();

        Container.Bind<UISwitcher>()
            .AsSingle()
            .NonLazy();
    }
}