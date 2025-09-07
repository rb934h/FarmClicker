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
        
        [HideInInspector]
        public GardenState State = GardenState.Empty;
        public bool canPlantSeed => currentSeed!=null;
        
        private CollectableItemData currentSeed; 
        private CollectableItemData harvestedSeed; 
        private TilemapAreaHighlighter _tilemapAreaHighlighter;
        private Color colorAfterWatering = new (.75f, .75f, .75f);
        private Color defaultColor = Color.white;
        private float seedingTime = .05f;


        private void Start()
        {
            _tilemapAreaHighlighter = new TilemapAreaHighlighter(tileMap, pointerObjectCollider);
            
            _fillBar.OnFill += StartRuinGarden;
            _fillBar.OnEmpty += Remove;
        }
        
        public CollectableItemData GetHarvestObject()
        {
            return harvestedSeed;
        }

        public void SetSeed(CollectableItemData seed)
        {
            if(harvestedSeed == null)
                currentSeed = seed;
        }
        
        public IEnumerator PlantSeed()
        {
            harvestedSeed = currentSeed;
            
            _fillBar.Show();

            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Seed));
            
            _fillBar.Filling(currentSeed.growTime + seedingTime);
            
            yield return new WaitForSeconds(currentSeed.growTime);
            
            State = GardenState.Planted;
            ShowStateInfo("Семя посажено, требуется полив.");
            
            hintSpriteRenderer.sprite = hintData.waterSprite;
            HintAnimator.Show(hintSpriteRenderer);
            
            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Sprout));
        }

        public IEnumerator WaterAndGrow()
        {
            if (currentSeed == null)
            {
                ShowStateInfo("Нет посаженного растения для полива.");
                yield break;
            }

            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Young));
            
            _tilemapAreaHighlighter.ChangeTilesColor(1, colorAfterWatering);
            
            ShowStateInfo("Полив начат, идет рост...");
            
            _fillBar.Show();
            _fillBar.Filling(currentSeed.growTime*2+seedingTime);
            
            yield return new WaitForSeconds(currentSeed.growTime);
            
            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Mature));
            
            yield return new WaitForSeconds(currentSeed.growTime);
            
            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Harvest));
            
            yield return new WaitForSeconds(seedingTime);
            
            ShowStateInfo("Рост завершен, готово к сбору.");
            
            State = GardenState.ReadyToHarvest;
            
            hintSpriteRenderer.sprite = hintData.harvestSprite;
            HintAnimator.Show(hintSpriteRenderer);
        }

        public void Remove()
        {
            if (currentSeed != null)
            {
                ClearSpritesFromSeedingPoints();
            }
            
            _fillBar.Hide();
            
            State = GardenState.Empty;
            
            HintAnimator.Hide(hintSpriteRenderer, true);
            
            currentSeed = null;
            harvestedSeed = null;
            
            _tilemapAreaHighlighter.ChangeTilesColor(1, defaultColor);
        }
        
        private void StartRuinGarden()
        {
            _fillBar.Emptying(currentSeed.ruinTime);
        }

        private IEnumerator SetSpritesSeedingPoints(GrowStates growState)
        {
            for (var i = 0; i < seedingPointsSpriteRenderers.Length; i++)
            {
                yield return new WaitForSeconds(seedingTime);
                seedingPointsSpriteRenderers[i].sprite = currentSeed.spritesForGarden[(int)growState];
            }
        }
        
        private void ClearSpritesFromSeedingPoints()
        {
            for (var i = 0; i < seedingPointsSpriteRenderers.Length; i++)
            {
                seedingPointsSpriteRenderers[i].sprite = null;
            }
        }
    }
}


