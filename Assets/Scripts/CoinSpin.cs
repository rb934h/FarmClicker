using UnityEngine;
using DG.Tweening;

public class CoinSpin
{
    private readonly SpriteRenderer _spriteRenderer;
    private readonly Sprite _frontSprite;
    private readonly Sprite _backSprite;
    private readonly float _duration;
    private readonly bool _loop;
    private readonly float _scaleMultiplier;

    private Sequence _sequence;

    public CoinSpin(
        SpriteRenderer spriteRenderer, 
        Sprite front, 
        Sprite back, 
        float duration = 0.6f, 
        bool loop = true,
        float scaleMultiplier = 1.25f)
    {
        _spriteRenderer = spriteRenderer;
        _frontSprite = front;
        _backSprite = back;
        _duration = duration;
        _loop = loop;
        _scaleMultiplier = scaleMultiplier;
    }

    public void Play()
    {
        if (_spriteRenderer == null) return;

        _spriteRenderer.enabled = true;
        Transform t = _spriteRenderer.transform;

        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        _sequence.Append(t.DOScaleX(0f, _duration / 2)
            .SetEase(Ease.InOutSine)
            .OnComplete(SwapSprite));

        _sequence.Append(t.DOScaleX(t.localScale.x * _scaleMultiplier, _duration / 2)
            .SetEase(Ease.InOutSine));

        _sequence.Insert(_duration / 4, 
            t.DOScaleY(t.localScale.y * _scaleMultiplier, _duration / 4)
             .SetEase(Ease.InOutSine)
             .SetLoops(2, LoopType.Yoyo));

        if (_loop)
            _sequence.SetLoops(-1, LoopType.Restart);
    }

    private void SwapSprite()
    {
        _spriteRenderer.sprite = 
            _spriteRenderer.sprite == _frontSprite ? _backSprite : _frontSprite;
    }
    
    public void Stop() => Clear();

    private void Clear()
    {
        _sequence?.Kill();
        _sequence = null;

        if (_spriteRenderer != null)
            _spriteRenderer.enabled = false;
    }
}
