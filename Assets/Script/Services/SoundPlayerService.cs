using UnityEngine;
using Zenject;

public class SoundPlayerService : ISoundPlayerService
{
    private readonly AudioSource audioSource;
    private readonly AudioClip openClip;
    private readonly AudioClip closeClip;

    [Inject]
    public SoundPlayerService(
        AudioSource audioSource,
        [Inject(Id = "OpenClip")] AudioClip openClip,
        [Inject(Id = "CloseClip")] AudioClip closeClip)
    {
        this.audioSource = audioSource;
        this.openClip = openClip;
        this.closeClip = closeClip;
    }

    public void PlayOpenSound()
    {
        if (openClip != null)
            audioSource.PlayOneShot(openClip);
    }

    public void PlayCloseSound()
    {
        if (closeClip != null)
            audioSource.PlayOneShot(closeClip);
    }
}
