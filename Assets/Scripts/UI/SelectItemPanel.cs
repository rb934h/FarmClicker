using ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class SelectItemPanel : MonoBehaviour
    {
        [SerializeField] private AddToInventoryButton addToInventoryButton;
    
        private  IObjectResolver _objectResolver;
   
        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        public void AddButton(CollectableItemData collectableItemData)
        {
            var instance = _objectResolver.Instantiate(addToInventoryButton, transform);
            instance.SetCollectableItemData(collectableItemData);
        }
    }
}
