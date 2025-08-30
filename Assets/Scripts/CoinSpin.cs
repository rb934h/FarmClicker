using UnityEngine;
using DG.Tweening;
using ScriptableObjects;

public class CoinSpin
{
    private readonly SpriteRenderer _spriteRenderer;
    private readonly Sprite _frontSprite;
    private readonly Sprite _backSprite;
    private readonly float _duration;
    private readonly bool _loop;
    private readonly float _scaleMultiplier;

    private Sequence _sequence;

    private Vector3 _defaultScale;

    public CoinSpin(
        SpriteRenderer spriteRenderer, 
        ChestData chestData)
    {
        _spriteRenderer = spriteRenderer;
        _frontSprite = chestData.coinFront;
        _backSprite = chestData.coinBack;
        _duration = chestData.coinSpinDuration;
        _loop = chestData.coinSpinAminationLoop;
        _scaleMultiplier = chestData.coinSpinScaleMultiplier;
    }

    public void Play()
    {
        if (_spriteRenderer == null) return;

        _spriteRenderer.enabled = true;
        Transform t = _spriteRenderer.transform;
        _defaultScale = t.localScale;

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
        
        _spriteRenderer.transform.localScale = _defaultScale;
    }
}
