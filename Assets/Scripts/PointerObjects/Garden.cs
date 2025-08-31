using System;
using System.Collections;
using Enum;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PointerObjects
{
    public class Garden : PointerObject
    {
        [SerializeField] private SpriteRenderer[] seedingPointsSpriteRenderers;
        [SerializeField] private SpriteRenderer hintSpriteRenderer;
        [SerializeField] private GardenHintData hintData;
        [SerializeField] private Tilemap tileMap;
        
        [HideInInspector]
        public GardenState State = GardenState.Empty;
        
        private CollectableItemData currentSeed; 
        private CollectableItemData harvestedSeed; 
        private TilemapAreaHighlighter _tilemapAreaHighlighter;
        private Color colorAfterWatering = new (.75f, .75f, .75f);
        private Color defaultColor = Color.white;


        private void Start()
        {
            _tilemapAreaHighlighter = new TilemapAreaHighlighter(tileMap, pointerObjectCollider);
        }

        public CollectableItemData GetHarvestObject()
        {
            return harvestedSeed;
        }
        
        public IEnumerator PlantSeed(CollectableItemData seed)
        {
            currentSeed = seed;
            
            if (currentSeed == null)
            {
                ShowStateInfo("Невозможно посадить: нет данных о семени.");
                yield break;
            }
            
            harvestedSeed = currentSeed;

            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Seed));
            
            yield return new WaitForSeconds(currentSeed.growTime);
            
            State = GardenState.Planted;
            ShowStateInfo("Семя посажено, требуется полив.");
            hintSpriteRenderer.sprite = hintData.waterSprite;
            
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
            
            yield return new WaitForSeconds(currentSeed.growTime);
            
            State = GardenState.ReadyToHarvest;
            hintSpriteRenderer.sprite = hintData.harvestSprite;
            
            ShowStateInfo("Рост завершен, готово к сбору.");
            
            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Mature));
            
            yield return new WaitForSeconds(currentSeed.growTime);
            
            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Harvest));
        }

        public void Harvest()
        {
            if (currentSeed != null)
            {
                ClearSpritesFromSeedingPoints();
            }
            
            ShowStateInfo("Урожай собран!");
            
            State = GardenState.Empty;
            
            hintSpriteRenderer.sprite = null;
            harvestedSeed = null;
            
            _tilemapAreaHighlighter.ChangeTilesColor(1, defaultColor);
        }

        private IEnumerator SetSpritesSeedingPoints(GrowStates growState)
        {
            for (var i = 0; i < seedingPointsSpriteRenderers.Length; i++)
            {
                yield return new WaitForSeconds(.05f);
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

