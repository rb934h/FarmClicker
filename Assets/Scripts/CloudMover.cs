using UnityEngine;
using DG.Tweening;

public class CloudMover : MonoBehaviour
{
    [SerializeField] private float startX = -10f;  // Позиция слева за кадром
    [SerializeField] private float endX = 10f;     // Позиция справа за кадром
    [SerializeField] private float yPos = 0f;      // Высота облака
    [SerializeField] private float moveTime = 10f; // Время движения через экран
    [SerializeField] private float repeatDelay = 2f; // Задержка перед повтором

    private void Start()
    {
        MoveCloud();
    }

    private void MoveCloud()
    {
        // Ставим облако в старт
        transform.position = new Vector3(startX, yPos, transform.position.z);

        // Запускаем анимацию
        transform.DOMoveX(endX, moveTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(repeatDelay, MoveCloud);
            });
    }
}