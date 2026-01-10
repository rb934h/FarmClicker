using System.Collections.Generic;
using DG.Tweening;
using PostProcessing;
using UnityEngine;
using VContainer;

namespace UI
{
    public abstract class BaseMenu : MonoBehaviour
    {
        [SerializeField] protected float _fadeDuration = 0.1f;
    
        private static int _openMenusCount = 0;
        private Tween _fadeTween;
        private URPVolume _urpVolume;
        private CanvasGroup _canvasGroup;
        protected static readonly List<BaseMenu> BaseMenus = new();
        protected bool IsOpen { get; private set; } = false;
        
    
        [Inject]
        public void Construct(URPVolume urpVolume)
        {
            _urpVolume = urpVolume;
        }
    

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            BaseMenus.Add(this);
        }

        protected virtual void OnDestroy()
        {
            _fadeTween?.Kill();
            BaseMenus.Remove(this);
        }

        protected virtual void Show()
        {
            if (IsOpen) return;

            IsOpen = true;
            _fadeTween?.Kill();
            _fadeTween = _canvasGroup.DOFade(1, _fadeDuration).SetUpdate(true);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _openMenusCount++;
        
            if (_openMenusCount == 1) 
            {
                ToggleEffects();
                Time.timeScale = 0f;
            }
        }

        protected void Hide()
        {
            if (!IsOpen) return;

            IsOpen = false;
            _fadeTween?.Kill();
            _fadeTween = _canvasGroup.DOFade(0, _fadeDuration).SetUpdate(true);
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
            if (IsOpen) Hide();
            else Show();
        }

        protected void SwitchTo(BaseMenu target)
        {
            target.Show();
            Hide();
        }
    }
}