using Enum;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAnimator
    {
        private Animator _animator;
        
        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void PlayAnimation(PlayerAnimationState state)
        {
            _animator.Play(state.ToString()); 
        }

    }
}