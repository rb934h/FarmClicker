using UnityEngine;

namespace DefaultNamespace 
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer leftHandItem;
        [SerializeField] private SpriteRenderer rightHandItem;
        
        public void SetItem(Sprite item)
        { 
            if(leftHandItem.sprite == null)
                leftHandItem.sprite = item;
            else
            {
                rightHandItem.sprite = item;
            }
        }
        
        public void ClearItem()
        {
            leftHandItem.sprite = null;
            rightHandItem.sprite = null;
        }
    }
}