using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PointerClickerTest : MonoBehaviour
{
    [FormerlySerializedAs("gardens")] [SerializeField] private PointerObject[] pointerObjects;
    
    Ray ray;
    public event Action<Vector3, PointerObject> OnPointerClick;
    
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        if (Input.GetMouseButtonDown(0))
        {
            foreach (var pointerObject in pointerObjects)
            {
                if (pointerObject.Collider != null && pointerObject.Collider.Raycast(ray, out hitData, 1000))
                {
                    if (pointerObject.IsAvailable)
                    {
                        OnPointerClick?.Invoke(pointerObject.PointForInteraction.position, pointerObject);
                    }
                       
                }
            }
        }
    }
}