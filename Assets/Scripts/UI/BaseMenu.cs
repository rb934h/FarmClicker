using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField] protected URPVolume _urpVolume;
    [SerializeField] protected float _fadeDuration = 0.1f;
    
    private CanvasGroup _canvasGroup;

    protected bool _isOpen = false;
    
    public bool IsOpen => _isOpen;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public virtual void Show(bool postProcessEffects = false)
    {
        _isOpen = true;
        _canvasGroup.DOFade(1, _fadeDuration);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        
        if (postProcessEffects)
            OnShow();
    }

    public virtual void Hide(bool postProcessEffects = false)
    {
        _isOpen = false;
        _canvasGroup.DOFade(0, _fadeDuration);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        
        if (postProcessEffects)
            OnHide();
    }

    protected virtual void OnShow()
    {
        ToggleEffects();
    }

    protected virtual void OnHide()
    {
        ToggleEffects();
    }
    
    private void ToggleEffects()
    {
        _urpVolume.ChangeChromaticAberrationValue(_fadeDuration);
        _urpVolume.ChangeDepthOfFieldValue(_fadeDuration);
    }
    
    public void Toggle()
    {
        if (_isOpen) Hide(true);
        else Show(true);
    }
}