using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using Enum;
using Level.Objects;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace PointerObjects
{
    public class Chest : PointerObject
    {
        [FormerlySerializedAs("chestData")] [SerializeField] private ChestData _chestData;
        [FormerlySerializedAs("objectsInChest")] [SerializeField] private SpriteRenderer[] _objectsInChest;
            
        [Header("Coin Spin")] 
        [SerializeField] private Transform _coinsParent;

        [Header("Sale board")] 
        [SerializeField] private SpriteRenderer _saleBoardSpriteRenderer;

        [Header("Tile changer")] 
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileReplacementRule _rule;
        
        [Space]
        [SerializeField] private FillBar _fillBar;

        [HideInInspector] 
        [FormerlySerializedAs("State")] public ChestState _state = ChestState.Empty;

        public event Action<List<CollectableItemData>> Solded;

        private TileChanger _tileChanger;
        private List<CollectableItemData> _cargo = new();
        private List<CoinSpin> _spins = new();
        private SaleBoard _saleBoard;
        private bool _waitForSale;

        public bool WaitForSale => _waitForSale;


        private void Start()
        {
            _tileChanger = new TileChanger(_tilemap, _rule);
            var coins = _coinsParent.GetComponentsInChildren<SpriteRenderer>();

            foreach (var coin in coins)
            {
                var spin = new CoinSpin(coin, _chestData);
                _spins.Add(spin);
            }

            _saleBoard = new SaleBoard(_saleBoardSpriteRenderer, _chestData);
        }

        public bool PutCargo(CollectableItemData objectsFromPlayer)
        {
            if(_cargo.Count > 1)
                return false;
            
            _cargo.Add(objectsFromPlayer);
            
            for (int chestIndex = 0; chestIndex < _cargo.Count; chestIndex++)
            {
               if(_objectsInChest[chestIndex].sprite == null)
                    _objectsInChest[chestIndex].sprite = objectsFromPlayer.spriteForHands;
            }
            
            return true;
        }

        public void ClearCargo()
        {
            Solded?.Invoke(_cargo);
            
            foreach (var spin in _spins)
            {
                spin.Stop();
            }

            _cargo.Clear();
        }
        
        public int GetCargoCount()
        {
            return _cargo.Count;
        }

        public void Send()
        {
            _waitForSale = true;
            _saleBoard.Play();
            _fillBar.Show();
            _fillBar.Filling(_chestData.deliveryTime + _chestData.saleBoardDuration);
            
            foreach (var collectableItemData in _cargo)
            {
                Debug.Log(collectableItemData.name);
            }
            
            foreach (var spriteRenderer in _objectsInChest)
            {
                spriteRenderer.sprite = null;
            }
            
            
            var seq = DOTween.Sequence();

            seq.AppendInterval(_chestData.deliveryTime);  
            seq.AppendCallback(() => _saleBoard.Stop()); 
            seq.AppendInterval(_chestData.saleBoardDuration);
            seq.AppendCallback(() =>
            {
                _waitForSale = false;
                _state = ChestState.WithMoney;
                ChangeTile();
                foreach (var spin in _spins)
                {
                    spin.Play();
                }
            }).OnComplete(_fillBar.Hide);
        }

        public void ChangeTile()
        {
            _tileChanger.ChangeTiles();
        }
    }
}
