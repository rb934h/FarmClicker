using Enum;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAnimation(PlayerAnimationState state)
        {
            _animator.Play(state.ToString());
        }
    }
}