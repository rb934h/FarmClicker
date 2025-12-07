using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CrossSceneAnimation : MonoBehaviour
{
    public static CrossSceneAnimation Instance { get; private set; }

    [SerializeField] private float _duration = 1f;

    private Material _material;
    private bool _isTransitioning = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        var image = Instance.GetComponentInChildren<Image>();

        _material = Instantiate(image.material);
        image.material = _material;
    }
    
    public async UniTaskVoid PlayTransition(Func<UniTask> loadOperation)
    {
        if (_isTransitioning || loadOperation == null)
            return;

        if (!EnsureMaterial())
            return;

        DOTween.Kill(_material);
        _isTransitioning = true;

        var token = this.GetCancellationTokenOnDestroy();

        _material.SetFloat("_ScreenAspect", (float)Screen.width / Screen.height);

        try
        {
            await PlayTween(
                _material.DOFloat(0f, "_Radius", _duration).SetEase(Ease.InOutSine),
                token
            );

            await loadOperation().AttachExternalCancellation(token);

            await PlayTween(
                _material.DOFloat(1f, "_Radius", _duration).SetEase(Ease.InOutSine),
                token
            );
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Token was cancelled");
        }
        finally
        {
            _isTransitioning = false;
        }
    }
    
    private UniTask PlayTween(Tween tween, CancellationToken token)
    {
        return tween
            .AsyncWaitForCompletion()
            .AsUniTask()
            .AttachExternalCancellation(token);
    }
    private bool EnsureMaterial()
    {
        if (_material != null)
            return true;

        var image = Instance.GetComponentInChildren<Image>(true);

        _material = Instantiate(image.material);
        image.material = _material;
        return true;
    }
}
