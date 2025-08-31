using UnityEngine;
using DG.Tweening;
using ScriptableObjects;

public class SaleBoard
{
    private readonly SpriteRenderer _spriteRenderer;
    private readonly float _fallDistance;
    private readonly float _duration;
    private readonly float _wobbleAngle;

    private Sequence _sequence;

    public SaleBoard(
        SpriteRenderer spriteRenderer,
        ChestData chestData)
    {
        _spriteRenderer = spriteRenderer;
        _spriteRenderer.sprite = chestData.saleBoard;
        _fallDistance = chestData.saleBoardFallDistance;
        _duration = chestData.saleBoardDuration;
        _wobbleAngle = chestData.saleBoardWobbleAngle;
    }

    public void Play()
    {
        if (_spriteRenderer == null) return;
        
        Transform t = _spriteRenderer.transform;
        Vector3 startPos = t.localPosition + Vector3.up * _fallDistance;
        Vector3 targetPos = t.localPosition;

        _sequence?.Kill();
        _sequence = DOTween.Sequence().SetAutoKill(false);

        t.localPosition = startPos;

        _sequence.AppendCallback(() => _spriteRenderer.enabled = !_spriteRenderer.enabled);
        
        _sequence.Append(t.DORotate(new Vector3(0, 0, _wobbleAngle), _duration*.25f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo));
        
        _sequence.Append(t.DOLocalMove(targetPos, _duration)
            .SetEase(Ease.InOutBack)); 

        
    }

    public void Stop() => Clear();
    private void Clear()
    {
        _sequence.PlayBackwards();
    }
}