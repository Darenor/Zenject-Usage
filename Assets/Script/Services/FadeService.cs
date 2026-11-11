using DG.Tweening;
using UnityEngine.UI;

public class FadeService : IFadeService
{
    public void FadeIn(Image image, float duration)
    {
        image.DOKill();
        image.gameObject.SetActive(true);

        image.color = new UnityEngine.Color(
            image.color.r,
            image.color.g,
            image.color.b,
            0f
            );

        image.DOFade(1f, duration);
    }

    public void FadeOut(Image image, float duration)
    {
        image.DOKill();

        image.DOFade(0f, duration)
            .OnComplete(() => image.gameObject.SetActive(false));
    }
}
