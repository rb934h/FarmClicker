using System;
using System.Collections;
using DefaultNamespace;
using Enum;
using ScriptableObjects;
using Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PointerObjects
{
    public class Garden : PointerObject
    {
        [SerializeField] private SpriteRenderer[] seedingPointsSpriteRenderers;
        [SerializeField] private SpriteRenderer hintSpriteRenderer;
        [SerializeField] private FillBar _fillBar;
        [SerializeField] private GardenHintData hintData;
        [SerializeField] private Tilemap tileMap;
        
        private CollectableItemData _currentSeed; 
        private CollectableItemData _harvestedSeed; 
        private TilemapAreaHighlighter _tilemapAreaHighlighter;
        private readonly Color _colorAfterWatering = new (.75f, .75f, .75f);
        private readonly Color _defaultColor = Color.white;
        private readonly float _seedingTime = .1f;
        private Coroutine _seedingCoroutine;
        
        [HideInInspector]
        public GardenState State = GardenState.Empty;
        public bool canPlantSeed => _currentSeed!=null;
        
        public event Action OnPlantSeed;
        
        private void Start()
        {
            _tilemapAreaHighlighter = new TilemapAreaHighlighter(tileMap, pointerObjectCollider);
            
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
            
            _workTime = _seedingTime * seedingPointsSpriteRenderers.Length;
            _fillBar.Filling(_workTime);
            
            yield return new WaitForSeconds(_currentSeed.growTime);
            
            State = GardenState.Planted;
            
            hintSpriteRenderer.sprite = hintData.waterSprite;
            HintAnimator.Show(hintSpriteRenderer);
            
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
            
            State = GardenState.ReadyToHarvest;
            
            hintSpriteRenderer.sprite = hintData.harvestSprite;
            HintAnimator.Show(hintSpriteRenderer);
        }

        public void Remove()
        {
            if (_currentSeed != null)
            {
                StartCoroutine(ClearSpritesFromSeedingPoints());
            }
            
            _workTime = _seedingTime * seedingPointsSpriteRenderers.Length;
            _fillBar.Hide();
            
            State = GardenState.Empty;
            
            HintAnimator.Hide(hintSpriteRenderer, true);
            
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
            foreach (var seedingPointSpriteRenderer in seedingPointsSpriteRenderers)
            {
                yield return new WaitForSeconds(_seedingTime);
                seedingPointSpriteRenderer.sprite = _currentSeed.spritesForGarden[(int)growState];
            }
        }
        
        private IEnumerator ClearSpritesFromSeedingPoints()
        {
            StopCoroutine(_seedingCoroutine);
            _seedingCoroutine = null;
            
            foreach (var seedingPointSpriteRenderer in seedingPointsSpriteRenderers)
            {
                yield return new WaitForSeconds(_seedingTime);
                seedingPointSpriteRenderer.sprite = null;
            }
        }
    }
}


