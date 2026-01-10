using DG.Tweening;
using UnityEngine;

namespace Level.Objects
{
    public class CloudMover : MonoBehaviour
    {
        [SerializeField] private float startX = -10f; 
        [SerializeField] private float endX = 10f;     
        [SerializeField] private float yPos = 0f;      
        [SerializeField] private float moveTime = 10f; 
        [SerializeField] private float repeatDelay = 2f;

        private void Start()
        {
            MoveCloud();
        }

        private void MoveCloud()
        {
            transform.position = new Vector3(startX, yPos, transform.position.z);
            transform.DOMoveX(endX, moveTime)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(repeatDelay, MoveCloud);
                });
        }
    }
}