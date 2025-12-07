using Cysharp.Threading.Tasks;
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
        
        private readonly float _secondsForWaitAfterAnimation = 2f;
        
        public async UniTask Show()
        {
            _animator.SetTrigger(ShowAnimationTrigger);
            await UniTask.WaitForSeconds(_secondsForWaitAfterAnimation);
        }

        public async UniTask Hide()
        {
            _animator.SetTrigger(HideAnimationTrigger);
            await UniTask.WaitForSeconds(_secondsForWaitAfterAnimation);
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