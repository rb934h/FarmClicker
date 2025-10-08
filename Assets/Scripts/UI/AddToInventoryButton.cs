using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class AddToInventoryButton : MonoBehaviour
{
   [SerializeField] private CollectableItemData _collectableItemData;
   [SerializeField] private AudioSource _clickSound;
   
   private Button _buttonComponent;
   private Player _player;
   
   [Inject]
   public void Construct(Player player)
   {
      _player = player;
   }
   private void Awake()
   {
      _buttonComponent = GetComponent<Button>();
      _buttonComponent.onClick.AddListener(()=>
      {
         _clickSound.Play();
         AddSeedToInventory(_collectableItemData);
      });
   }

   
   private void AddSeedToInventory(CollectableItemData item)
   {
      _player.inventory.currentSeed = item;
   }
}
