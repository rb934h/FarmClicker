using UnityEngine;
using System;

public class PointerClickerTest : MonoBehaviour
{
    [SerializeField] private PointerObject[] pointerObjects;

    public event Action<Vector2, PointerObject> OnPointerClick;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(Camera.main.transform.position.z);  // чтобы вернуть в z = 0
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

            foreach (var pointerObject in pointerObjects)
            {
                if (pointerObject.IsAvailable && 
                    pointerObject.Collider != null &&
                    pointerObject.Collider.OverlapPoint(worldPoint))
                {
                    OnPointerClick?.Invoke(pointerObject.PointForInteraction.position, pointerObject);
                }
            }
        }

    }
}