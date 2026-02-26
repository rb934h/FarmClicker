using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace UI
{
    public class SelectItemPanel : MonoBehaviour
    {
        [FormerlySerializedAs("addToInventoryButton")] [SerializeField] private AddToInventoryButton _addToInventoryButton;
    
        private  IObjectResolver _objectResolver;
   
        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        
        public void AddButton(CollectableItemData collectableItemData)
        {
            var instance = _objectResolver.Instantiate(_addToInventoryButton, transform);
            instance.SetCollectableItemData(collectableItemData);
        }
    }
}
