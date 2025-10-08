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
                coinsCountText.text = $"{_player.inventory.coins}";
            }
        }
    }
}