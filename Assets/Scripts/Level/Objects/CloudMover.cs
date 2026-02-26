using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level.Objects
{
    public class CloudMover : MonoBehaviour
    {
        [FormerlySerializedAs("startX")] [SerializeField] private float _startX = -10f;
        [FormerlySerializedAs("endX")] [SerializeField] private float _endX = 10f;
        [FormerlySerializedAs("yPos")] [SerializeField] private float _yPos = 0f;
        [FormerlySerializedAs("moveTime")] [SerializeField] private float _moveTime = 10f;
        [FormerlySerializedAs("repeatDelay")] [SerializeField] private float _repeatDelay = 2f;

        private void Start()
        {
            MoveCloud();
        }

        private void MoveCloud()
        {
            transform.position = new Vector3(_startX, _yPos, transform.position.z);
            transform.DOMoveX(_endX, _moveTime)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(_repeatDelay, MoveCloud);
                });
        }
    }
}
