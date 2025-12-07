using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class AddToInventoryButton : MonoBehaviour
    {
        [SerializeField] private Image _collectableItemIcon;
        [SerializeField] private AudioSource _clickSound;

        private CollectableItemData _collectableItemData;
        private Button _buttonComponent;
        private Player _player;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
        }

        public void SetCollectableItemData(CollectableItemData collectableItemData)
        {
            _collectableItemData = collectableItemData;

            EnableButton();
        }

        private void EnableButton()
        {
            if (_collectableItemData is not (PlantData or VegetableData))
            {
                Debug.Log("Wrong collectableItemData type");
                return;
            }

            _collectableItemIcon.sprite = _collectableItemData.spriteForHands;

            _buttonComponent = GetComponent<Button>();
            _buttonComponent.onClick.AddListener(() =>
            {
                // _clickSound.Play();
                AddSeedToInventory(_collectableItemData);
            });
        }

        private void AddSeedToInventory(CollectableItemData item)
        {
            _player.inventory.currentSeed = item;
        }
    }
}