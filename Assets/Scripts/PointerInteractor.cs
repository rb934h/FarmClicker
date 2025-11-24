using UnityEngine;
using System;
using PointerObjects;

public class PointerInteractor : MonoBehaviour
{
    [SerializeField] private PointerObject[] pointerObjects;
    [SerializeField] private AudioSource _clickSound;
    
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
            var mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(_camera.transform.position.z);
            var worldPoint = _camera.ScreenToWorldPoint(mousePosition);

            foreach (var pointerObject in pointerObjects)
            {
                if (pointerObject.Collider != null &&
                    pointerObject.Collider.OverlapPoint(worldPoint))
                {
                    _clickSound?.Play();
                    PointerClicked?.Invoke(pointerObject.PointForInteraction.position, pointerObject);
                }
            }
        }
    }
}