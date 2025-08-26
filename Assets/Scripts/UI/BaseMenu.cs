using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField] protected URPVolume _urpVolume;
    [SerializeField] protected float _fadeDuration = 0.1f;
    
    private CanvasGroup _canvasGroup;
    protected static List<BaseMenu> BaseMenus = new();
    private static int _openMenusCount = 0;

    private bool _isOpen = false;
    protected bool IsOpen => _isOpen;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        BaseMenus.Add(this);
    }

    protected virtual void OnDestroy()
    {
        BaseMenus.Remove(this);
    }

    protected virtual void Show()
    {
        if (_isOpen) return;

        _isOpen = true;
        _canvasGroup.DOFade(1, _fadeDuration).SetUpdate(true);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        _openMenusCount++;
        
        if (_openMenusCount == 1) 
        {
            ToggleEffects();
            Time.timeScale = 0f;
        }
    }

    protected virtual void Hide()
    {
        if (!_isOpen) return;

        _isOpen = false;
        _canvasGroup.DOFade(0, _fadeDuration).SetUpdate(true);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _openMenusCount--;
        
        if (_openMenusCount == 0)
        {
            ToggleEffects();
            Time.timeScale = 1f;
        }
    }

    private void ToggleEffects()
    {
        if (_urpVolume == null) return;
        
        _urpVolume.ChangeChromaticAberrationValue(_fadeDuration);
        _urpVolume.ChangeDepthOfFieldValue(_fadeDuration);
    }

    protected void Toggle()
    {
        if (_isOpen) Hide();
        else Show();
    }

    protected void SwitchTo(BaseMenu target)
    {
        target.Show();
        Hide();
    }

}