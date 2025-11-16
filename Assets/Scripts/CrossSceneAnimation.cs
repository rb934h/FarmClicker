using System;
using System.Collections;
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
    
    public void Play(Func<AsyncOperation> loadOperation)
    {
        if (_isTransitioning || loadOperation == null)
            return;

        if (!EnsureMaterial())
            return;

        _isTransitioning = true;

        _material.SetFloat("_ScreenAspect", (float)Screen.width / Screen.height);
        
        _material
            .DOFloat(0f, "_Radius", _duration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .OnComplete(() => StartCoroutine(PlaySecondHalf(loadOperation)));
    }
    
    private IEnumerator PlaySecondHalf(Func<AsyncOperation> loadOperation)
    {
        var asyncOp = loadOperation.Invoke();
        
        if (asyncOp != null)
        {
            while (!asyncOp.isDone)
                yield return null;
        }
        else
        {
            yield return null;
        }

        if (!EnsureMaterial())
        {
            _isTransitioning = false;
            yield break;
        }
        
        _material
            .DOFloat(1f, "_Radius", _duration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .OnComplete(() => _isTransitioning = false);
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
