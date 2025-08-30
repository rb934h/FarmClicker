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

        public void PutCargo(List<CollectableItemData> objectsFromPlayer)
        {
            cargo.AddRange(objectsFromPlayer);
            
            for (var i = 0; i < objectsFromPlayer.Count; i++)
            {
                objectsInChest[i].sprite = objectsFromPlayer[i].spriteForHands;
            }
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

            DOVirtual.DelayedCall(chestData.deliveryTime,
                () =>
                {
                    State = ChestState.WithMoney;
                    foreach (var spin in _spins)
                    {
                        spin.Play();
                    }

                    _saleBoard.Stop();
                    ChangeTile();
                });
        }

        public void ChangeTile()
        {
            _tileChanger.ChangeTiles();
        }
    }
}