using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Azur.Playable.Carousel
{
    public class CarouselUIService
    {
        internal readonly bool IsCanvas;
        private readonly List<Transform> _items;

        public CarouselUIService(bool isCanvas, List<Transform> items)
        {
            IsCanvas = isCanvas;
            _items = items;
            
            RequestSortingOrderUpdate();
        }

        internal void RequestSortingOrderUpdate()
        {
            if (IsCanvas == false)
            {
                return;
            }
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                EditorApplication.delayCall += UpdateSortingOrderByZPosition;
            }
            else
#endif
            {
                UpdateSortingOrderByZPosition();
            }
        }
        
        private void UpdateSortingOrderByZPosition()
        {
            if (_items == null || _items.Count == 0)
            {
                return;
            }
            
            var sortedItems = _items
                .Where(item => item != null && item.gameObject != null)
                .OrderByDescending(item => item.position.z)
                .ToList();

            for (var itemIndex = 0; itemIndex < sortedItems.Count; itemIndex++)
            {
                var item = sortedItems[itemIndex];
                
                if (item.TryGetComponent<Canvas>(out var canvas) == false)
                {
                    canvas = item.gameObject.AddComponent<Canvas>();
                }

                canvas.overrideSorting = true;
                canvas.sortingOrder = itemIndex;
            }
        }
    }
}