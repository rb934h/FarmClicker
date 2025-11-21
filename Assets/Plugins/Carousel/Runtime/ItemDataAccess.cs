using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Azur.Playable.Carousel
{
    public class ItemDataAccess
    {
        internal List<Transform> items { get; private set; }
        internal Vector3 ItemScale;
        internal readonly float ItemsDepth;
        
        private readonly float _itemSpacing;
        private List<Transform> _leftColumnItems = new List<Transform>();
        private List<Transform> _rightColumnItems = new List<Transform>();

        public ItemDataAccess(float itemsDepth, float itemSpacing, List<Transform> carouselChildren)
        {
            ItemsDepth = itemsDepth;
            items = carouselChildren;
            _itemSpacing = itemSpacing;
            
            ArrangeItems();
        }

        internal void Reverse()
        {
            SplitItemGroups();
            
            var newPositions = new List<Transform>();
            newPositions.AddRange(_rightColumnItems);
            newPositions.Add(items[0]);
            newPositions.AddRange(_leftColumnItems);
            newPositions.Reverse();
            items = newPositions;
        }
        
        private void ArrangeItems()
        {
            ConfigureFirstItem();
            SplitItemGroups();
            
            var currentDeep = ItemsDepth;
            var deepStep = ItemsDepth;
            
            foreach (var item in _leftColumnItems)
            {
                item.localPosition = new Vector3(
                    items[0].localPosition.x - _itemSpacing,
                    items[0].localPosition.y,
                    currentDeep);
                currentDeep += deepStep;
            }

            currentDeep = ItemsDepth;

            _rightColumnItems.Reverse();
            foreach (var item in _rightColumnItems)
            {
                item.localPosition = new Vector3(
                    items[0].localPosition.x + _itemSpacing,
                    items[0].localPosition.y,
                    currentDeep);
                currentDeep += deepStep;
            }
        }
        
        private void ConfigureFirstItem()
        {
            if(items == null || items.Count == 0)
                return;
            
            ItemScale = items.First().localScale;
            items.First().localPosition = Vector3.zero;
        }
        
        private void SplitItemGroups()
        {
            var subList = items.Skip(1).ToList();
            var mid = subList.Count / 2;
            _leftColumnItems = subList.GetRange(0, mid);
            _rightColumnItems = subList.GetRange(mid, subList.Count - mid);
        }
    }
}