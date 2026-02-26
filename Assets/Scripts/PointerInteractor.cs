using UnityEngine;
using System;
using PointerObjects;
using UnityEngine.Serialization;

public class PointerInteractor : MonoBehaviour
{
    [FormerlySerializedAs("pointerObjects")] [SerializeField] private PointerObject[] _pointerObjects;
    [SerializeField] private AudioSource _clickSound;
    
    private Camera _camera;

    public event Action<Vector2, PointerObject> PointerClicked;
    public PointerObject[] PointerObjects => _pointerObjects;

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

            foreach (var pointerObject in _pointerObjects)
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
