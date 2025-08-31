using System;
using System.Collections.Generic;
using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PointerObjects
{
    public class Chest : PointerObject
    {
        [SerializeField] private ChestData chestData;
        [SerializeField] private SpriteRenderer[] objectsInChest;
            
        [Header("Coin Spin")] 
        [SerializeField] private Transform _coinsParent;

        [Header("Sale board")] 
        [SerializeField] private SpriteRenderer _saleBoardSpriteRenderer;

        [Header("Tile changer")] 
        [SerializeField] private Tilemap _tilemap;

        [SerializeField] private TileReplacementRule _rule;

        [HideInInspector] 
        public ChestState State = ChestState.Empty;

        public event Action<List<CollectableItemData>> IsSolded;

        private TileChanger _tileChanger;
        private List<CollectableItemData> cargo = new();
        private List<CoinSpin> _spins = new();
        private SaleBoard _saleBoard;


        private void Start()
        {
            _tileChanger = new TileChanger(_tilemap, _rule);
            var coins = _coinsParent.GetComponentsInChildren<SpriteRenderer>();

            foreach (var coin in coins)
            {
                var spin = new CoinSpin(coin, chestData);
                _spins.Add(spin);
            }

            _saleBoard = new SaleBoard(_saleBoardSpriteRenderer, chestData);
        }

        public bool PutCargo(CollectableItemData objectsFromPlayer)
        {
            if(cargo.Count > 1)
                return false;
            
            cargo.Add(objectsFromPlayer);
            
            for (int chestIndex = 0; chestIndex < cargo.Count; chestIndex++)
            {
               if(objectsInChest[chestIndex].sprite == null)
                    objectsInChest[chestIndex].sprite = objectsFromPlayer.spriteForHands;
            }
            
            return true;
        }

        public void ClearCargo()
        {
            IsSolded?.Invoke(cargo);
            
            foreach (var spin in _spins)
            {
                spin.Stop();
            }

            cargo.Clear();
        }

        public float GetCargoPrice()
        {
            foreach (var collectableItemData in cargo)
            {
                return collectableItemData.price;
            }

            return 0;
        }
        
        public int GetCargoCount()
        {
            return cargo.Count;
        }

        public void Send()
        {
            _saleBoard.Play();

            ShowStateInfo("Машина отправлена");
            foreach (var collectableItemData in cargo)
            {
                Debug.Log(collectableItemData.name);
            }
            
            foreach (var spriteRenderer in objectsInChest)
            {
                spriteRenderer.sprite = null;
            }

            var seq = DOTween.Sequence();

            seq.AppendInterval(chestData.deliveryTime);  
            seq.AppendCallback(() => _saleBoard.Stop()); 
            seq.AppendInterval(chestData.saleBoardDuration);                    
            seq.AppendCallback(() =>
            {
                State = ChestState.WithMoney;
                ChangeTile();
                foreach (var spin in _spins)
                {
                    spin.Play();
                }
            });
        }

        public void ChangeTile()
        {
            _tileChanger.ChangeTiles();
        }
    }
}