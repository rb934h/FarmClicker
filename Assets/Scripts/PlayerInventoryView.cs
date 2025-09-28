using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using VContainer;

namespace DefaultNamespace 
{
    public class PlayerInventoryView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer leftHandItem;
        [SerializeField] private SpriteRenderer rightHandItem;
        [SerializeField] private SpriteRenderer wateringCan;
        [SerializeField] private TMP_Text coinsCountText;

        private PlayerInventoryData _playerInventory;
        private CollectableItemData _leftHandItemData;
        private CollectableItemData _rightHandItemData;

        [Inject]
        public void Construct(PlayerInventoryData playerInventoryData)
        {
            _playerInventory = playerInventoryData;
        }

        private void Start()
        {
            _playerInventory.OnWaterFilled += ShowWateringCan;
            _playerInventory.OnWaterUsed += HideWateringCan;
            _playerInventory.HarvestObjectAdded += SetItem;
            _playerInventory.HarvestObjectsClear += ClearItem;
            _playerInventory.OnClearAllItems += ClearAllItems;
            _playerInventory.CoinsChanged += UpdateCoinsCountText;
        }

        private void HideWateringCan()
        {
            wateringCan.enabled = false;
        }

        private void ShowWateringCan()
        {
            wateringCan.enabled = true;
        }

        private void SetItem(CollectableItemData item)
        {
            if (rightHandItem.sprite == null)
            {
                rightHandItem.sprite = item.spriteForHands;
                _rightHandItemData = item;
            }
            else
            {
                leftHandItem.sprite = item.spriteForHands;
                _leftHandItemData = item;
            }
        }
        
        private void ClearItem(CollectableItemData item)
        {
            if (_leftHandItemData == item)
            {
                leftHandItem.sprite = null;
                _leftHandItemData = null;
            }
            else if (_rightHandItemData == item)
            {
                rightHandItem.sprite = null;
                _rightHandItemData = null;
            }
        }

        private void ClearAllItems()
        {
            leftHandItem.sprite = null;
            _leftHandItemData = null;
            rightHandItem.sprite = null;
            _rightHandItemData = null;
        }
        
        private void UpdateCoinsCountText()
        {
            if (coinsCountText != null)
            {
                coinsCountText.text = $"Coins: {_playerInventory.coins}";
            }
        }
    }
}