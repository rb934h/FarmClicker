using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class CrossSceneAnimation : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;

    private Material _material;
    private bool _isTransitioning = false;

    private void Awake()
    {
        var image = GetComponentInChildren<Image>();
        
        _material = Instantiate(image.material);
        image.material = _material;
        
        DontDestroyOnLoad(gameObject);
    }

    public void Play(Action loadLevel)
    {
        if (_isTransitioning) return;
        _isTransitioning = true;
       
        _material.SetFloat("_ScreenAspect", (float)Screen.width / Screen.height);
        _material.DOFloat(0f, "_Radius", _duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                StartCoroutine(PlaySecondHalf(loadLevel));
            });
    }

    private IEnumerator PlaySecondHalf(Action loadLevel)
    {
        loadLevel?.Invoke();
        
        yield return null;
        
        _material.DOFloat(1, "_Radius", _duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _isTransitioning = false;
            });
    }
}