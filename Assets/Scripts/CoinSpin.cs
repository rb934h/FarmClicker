using UnityEngine;
using DG.Tweening;

public class CoinSpin : Component
{
    private Sprite frontSprite;
    private Sprite backSprite; 
    private float duration = 0.6f; 
    private bool loop = true;
    

    private void AnimateCoin(SpriteRenderer spriteRenderer)
    {
        Sequence seq = DOTween.Sequence();
        
        seq.Append(transform.DOScaleX(0f, duration / 2)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                spriteRenderer.sprite = spriteRenderer.sprite == frontSprite ? backSprite : frontSprite;
            }));
        
        seq.Append(transform.DOScaleX(transform.localScale.x*1.25f, duration / 2).SetEase(Ease.InOutSine));
        
        seq.Insert(duration / 4, transform.DOScaleY(transform.localScale.y*1.25f, duration / 4)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo)); 

        if (loop)
            seq.SetLoops(-1, LoopType.Restart);
    }
}