using System;
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

        private void SetItem(Sprite item)
        {   
            if(leftHandItem.sprite == null)
                leftHandItem.sprite = item;
            else
            {
                rightHandItem.sprite = item;
            }
        }
        
        private void ClearItem()
        {
            leftHandItem.sprite = null;
            rightHandItem.sprite = null;
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