using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class AddToInventoryButton : MonoBehaviour
{
   [SerializeField] private CollectableItemData _collectableItemData;
   [SerializeField] private AudioSource _clickSound;
   
   private Button buttonComponent;
   private PlayerInventoryData playerInventory;
   
   [Inject]
   public void Construct(PlayerInventoryData playerInventoryData)
   {
      playerInventory = playerInventoryData;
   }
   private void Awake()
   {
      buttonComponent = GetComponent<Button>();
      buttonComponent.onClick.AddListener(()=>
      {
         _clickSound.Play();
         AddSeedToInventory(_collectableItemData);
      });
   }

   
   private void AddSeedToInventory(CollectableItemData item)
   {
      playerInventory.currentSeed = item;
   }
}
