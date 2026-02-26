using DG.Tweening;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Player
{
    public class PlayerInventoryView : MonoBehaviour
    {
        [FormerlySerializedAs("leftHandItem")] [SerializeField] private SpriteRenderer _leftHandItem;
        [FormerlySerializedAs("rightHandItem")] [SerializeField] private SpriteRenderer _rightHandItem;
        [FormerlySerializedAs("wateringCan")] [SerializeField] private SpriteRenderer _wateringCan;
        [FormerlySerializedAs("coinsCountText")] [SerializeField] private TMP_Text _coinsCountText;

        private Player _player;
        private CollectableItemData _leftHandItemData;
        private CollectableItemData _rightHandItemData;

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
        }

        private void Start()
        {
            _player.inventory.OnWaterFilled += ShowWateringCan;
            _player.inventory.OnWaterUsed += HideWateringCan;
            _player.inventory.HarvestObjectAdded += SetItem;
            _player.inventory.HarvestObjectsClear += ClearItem;
            _player.inventory.OnClearAllItems += ClearAllItems;
            _player.inventory.CoinsChanged += UpdateCoinsCountText;
        }

        private void HideWateringCan()
        {
            _wateringCan.enabled = false;
        }

        private void ShowWateringCan()
        {
            _wateringCan.enabled = true;
        }

        private void SetItem(CollectableItemData item)
        {
            if (_wateringCan.enabled)
            {
                HideWateringCan();
                DOVirtual.DelayedCall(1f, ShowWateringCan);
            }
               
            
            if (_rightHandItem.sprite == null)
            {
                _rightHandItem.sprite = item.spriteForHands;
                _rightHandItemData = item;
            }
            else
            {
                _leftHandItem.sprite = item.spriteForHands;
                _leftHandItemData = item;
            }
        }

        private void ClearItem(CollectableItemData item)
        {
            if (_leftHandItemData == item)
            {
                _leftHandItem.sprite = null;
                _leftHandItemData = null;
            }
            else if (_rightHandItemData == item)
            {
                _rightHandItem.sprite = null;
                _rightHandItemData = null;
            }
        }

        private void ClearAllItems()
        {
            _leftHandItem.sprite = null;
            _leftHandItemData = null;
            _rightHandItem.sprite = null;
            _rightHandItemData = null;
        }

        private void UpdateCoinsCountText()
        {
            if (_coinsCountText != null)
            {
                _coinsCountText.text = $"{_player.inventory.coins}";
            }
        }
    }
}
