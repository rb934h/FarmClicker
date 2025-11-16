using UnityEngine;
using System;
using PointerObjects;
using UnityEngine.EventSystems;

public class PointerInteractor : MonoBehaviour
{
    [SerializeField] private PointerObject[] pointerObjects;
    private Camera _camera;

    public event Action<Vector2, PointerObject> PointerClicked;
    public PointerObject[] PointerObjects => pointerObjects;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(_camera.transform.position.z);
            Vector2 worldPoint = _camera.ScreenToWorldPoint(mousePosition);

            foreach (var pointerObject in pointerObjects)
            {
                if (pointerObject.Collider != null &&
                    pointerObject.Collider.OverlapPoint(worldPoint))
                {
                    PointerClicked?.Invoke(pointerObject.PointForInteraction.position, pointerObject);
                }
            }
        }
    }
}