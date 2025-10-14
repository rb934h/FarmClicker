using System.Collections.Generic;
using UnityEngine;

public class SortingOrderManager : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _renderers = new();
    
    private void LateUpdate()
    {
        foreach (var r in _renderers)
        {
            r.sortingOrder = Mathf.RoundToInt(-r.transform.position.y * 100);
        }
    }
}