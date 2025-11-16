using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class Convert : MonoBehaviour
    {
        private const string ShowAnimationTrigger = "Show";
        private const string HideAnimationTrigger = "Hide";
        
        [SerializeField] private TMP_Text _tmpTextMessage;
        [SerializeField] private TMP_Text _tmpTextMessageSender;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _letterSound;

        public void Show()
        {
            _animator.SetTrigger(ShowAnimationTrigger);
        }

        public void Hide()
        {
            _animator.SetTrigger(HideAnimationTrigger);
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