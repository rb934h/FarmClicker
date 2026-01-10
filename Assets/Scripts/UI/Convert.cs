using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Convert : MonoBehaviour
    {
        private const string ShowAnimationTrigger = "Show";
        private const string HideAnimationTrigger = "Hide";
        
        [SerializeField] private TMP_Text _tmpTextMessage;
        [SerializeField] private TMP_Text _tmpTextMessageSender;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _letterSound;
        
        private readonly float _secondsForWaitAfterAnimation = 2f;

        public event Action OnShowed;
        public event Action OnHided;
        
        
        public void Show()
        {
            _animator.SetTrigger(ShowAnimationTrigger);
            DOVirtual.DelayedCall(_secondsForWaitAfterAnimation, () =>
            {
                OnShowed?.Invoke();
            });
        }

        public void Hide()
        {
            _animator.SetTrigger(HideAnimationTrigger);
            DOVirtual.DelayedCall(_secondsForWaitAfterAnimation, () =>
            {
                OnHided?.Invoke();
            });
        }

        public void PlayLetterSound()
        {
            _letterSound?.Play();
        }
        public void SetMessage(string message)
        {
            _tmpTextMessage.text = message;
        }

        public void SetMessageSender(string sender)
        {
            _tmpTextMessageSender.text = sender;
        }
    }
}