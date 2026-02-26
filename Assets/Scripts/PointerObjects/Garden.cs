using System;
using System.Collections;
using DefaultNamespace;
using Enum;
using ScriptableObjects;
using Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace PointerObjects
{
    public class Garden : PointerObject
    {
        [FormerlySerializedAs("seedingPointsSpriteRenderers")] [SerializeField] private SpriteRenderer[] _seedingPointsSpriteRenderers;
        [FormerlySerializedAs("hintSpriteRenderer")] [SerializeField] private SpriteRenderer _hintSpriteRenderer;
        [SerializeField] private FillBar _fillBar;
        [FormerlySerializedAs("hintData")] [SerializeField] private GardenHintData _hintData;
        [FormerlySerializedAs("tileMap")] [SerializeField] private Tilemap _tileMap;
        
        private CollectableItemData _currentSeed; 
        private CollectableItemData _harvestedSeed; 
        private TilemapAreaHighlighter _tilemapAreaHighlighter;
        private readonly Color _colorAfterWatering = new (.75f, .75f, .75f);
        private readonly Color _defaultColor = Color.white;
        private readonly float _seedingTime = .1f;
        private Coroutine _seedingCoroutine;
        
        [HideInInspector]
        [FormerlySerializedAs("State")] public GardenState _state = GardenState.Empty;
        public bool canPlantSeed => _currentSeed!=null;
        
        public event Action OnPlantSeed;
        
        private void Start()
        {
            _tilemapAreaHighlighter = new TilemapAreaHighlighter(_tileMap, _pointerObjectCollider);
            
            _fillBar.OnFill += StartRuinGarden;
            _fillBar.OnEmpty += Remove;
        }
        
        public CollectableItemData GetHarvestObject()
        {
            return _harvestedSeed;
        }

        public void SetSeed(CollectableItemData seed)
        {
            if(_harvestedSeed == null)
                _currentSeed = seed;
        }
        
        public IEnumerator PlantSeed()
        {
            _harvestedSeed = _currentSeed;
            
            _fillBar.Show();

            _seedingCoroutine = StartCoroutine(SetSpritesSeedingPoints(GrowStates.Seed));
            
            _workTime = _seedingTime * _seedingPointsSpriteRenderers.Length;
            _fillBar.Filling(_workTime);
            
            yield return new WaitForSeconds(_currentSeed.growTime);
            
            _state = GardenState.Planted;
            
            _hintSpriteRenderer.sprite = _hintData.waterSprite;
            HintAnimator.Show(_hintSpriteRenderer);
            
            _seedingCoroutine = StartCoroutine(SetSpritesSeedingPoints(GrowStates.Sprout));

            OnPlantSeed?.Invoke();
        }

        public IEnumerator WaterAndGrow()
        {
            if (_currentSeed == null)
            {
                yield break;
            }

            _seedingCoroutine = StartCoroutine(SetSpritesSeedingPoints(GrowStates.Young));
            
            _tilemapAreaHighlighter.ChangeTilesColor(1, _colorAfterWatering);
            
            _fillBar.Show();
            _workTime = _currentSeed.growTime * 2 + _seedingTime;
            _fillBar.Filling(_workTime);
            
            yield return new WaitForSeconds(_currentSeed.growTime);
            
            _seedingCoroutine = StartCoroutine(SetSpritesSeedingPoints(GrowStates.Mature));
            
            yield return new WaitForSeconds(_currentSeed.growTime);
            
            _seedingCoroutine = StartCoroutine(SetSpritesSeedingPoints(GrowStates.Harvest));
            
            yield return new WaitForSeconds(_seedingTime);
            
            _state = GardenState.ReadyToHarvest;
            
            _hintSpriteRenderer.sprite = _hintData.harvestSprite;
            HintAnimator.Show(_hintSpriteRenderer);
        }

        public void Remove()
        {
            if (_currentSeed != null)
            {
                StartCoroutine(ClearSpritesFromSeedingPoints());
            }
            
            _workTime = _seedingTime * _seedingPointsSpriteRenderers.Length;
            _fillBar.Hide();
            
            _state = GardenState.Empty;
            
            HintAnimator.Hide(_hintSpriteRenderer, true);
            
            _currentSeed = null;
            _harvestedSeed = null;
            
            _tilemapAreaHighlighter.ChangeTilesColor(1, _defaultColor);
        }
        
        private void StartRuinGarden()
        {
            _fillBar.Emptying(_currentSeed.ruinTime);
        }

        private IEnumerator SetSpritesSeedingPoints(GrowStates growState)
        {
            foreach (var seedingPointSpriteRenderer in _seedingPointsSpriteRenderers)
            {
                yield return new WaitForSeconds(_seedingTime);
                seedingPointSpriteRenderer.sprite = _currentSeed.spritesForGarden[(int)growState];
            }
        }
        
        private IEnumerator ClearSpritesFromSeedingPoints()
        {
            StopCoroutine(_seedingCoroutine);
            _seedingCoroutine = null;
            
            foreach (var seedingPointSpriteRenderer in _seedingPointsSpriteRenderers)
            {
                yield return new WaitForSeconds(_seedingTime);
                seedingPointSpriteRenderer.sprite = null;
            }
        }
    }
}


