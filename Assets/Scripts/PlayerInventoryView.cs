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
            _playerInventory.HarvestObjectAdded += SetItem;
            _playerInventory.HarvestObjectsClear += ClearItem;
            _playerInventory.CoinsChanged += UpdateCoinsCountText;
        }

        private void SetItem(CollectableItemData item)
        {
            if (leftHandItem.sprite == null)
            {
                leftHandItem.sprite = item.spriteForHands;
                _leftHandItemData = item;
            }
                
            else
            {
                rightHandItem.sprite = item.spriteForHands;
                _rightHandItemData = item;
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
        
        private void UpdateCoinsCountText()
        {
            if (coinsCountText != null)
            {
                coinsCountText.text = $"Coins: {_playerInventory.coins}";
            }
        }
    }
}