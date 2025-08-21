using System.Collections;
using Enum;
using ScriptableObjects;
using UnityEngine;


public class Garden : PointerObject
    {
        [SerializeField] private SpriteRenderer[] seedingPointsSpriteRenderers;
        [SerializeField] private SpriteRenderer hintSpriteRenderer;
        [SerializeField] private HintData hintData;

        private CollectableItemData seedingScriptableObject; 
        private CollectableItemData harvestScriptableObject; 
        
        private GameObject currentSeedingInstance; 
        private GardenState currentState = GardenState.Empty;
        private Color gardenColorAfterWaterSeed;
        private Color gardenDefaultColor;
        
        
        public CollectableItemData SeedingScriptableObject => seedingScriptableObject;
        public GardenState State => currentState;


        public CollectableItemData GetHarvestObject()
        {
            return harvestScriptableObject;
        }
        private void Start()
        {
            gardenColorAfterWaterSeed = new Color(0.7f, 0.5f, 0.35f);
        }
        public override void ChangeState()
        {
            if(!IsAvailable)
                return;
            
            IsAvailable = false;
            
            switch (currentState)
            {
                case GardenState.Empty:
                    StartCoroutine(PlantSeed());
                    break;
                case GardenState.Planted:
                    StartCoroutine(WaterAndGrow());
                    break;
                case GardenState.ReadyToHarvest:
                    Harvest();
                    break;
                default:
                    Debug.Log("Нет доступного действия для текущего состояния: " + currentState);
                    break;
            }
        }
        
        public void SetSeedingData(CollectableItemData seedingData)
        {
            seedingScriptableObject = seedingData;
        }
        
        private IEnumerator PlantSeed()
        {
            if (seedingScriptableObject == null)
            {
                ShowStateInfo("Невозможно посадить: нет данных о семени.");
                IsAvailable = true;
                yield break;
            }
            
            harvestScriptableObject = seedingScriptableObject;

            SetSpritesSeedingPoints(GrowStates.Seed);
            
            yield return new WaitForSeconds(seedingScriptableObject.growTime);
            
            currentState = GardenState.Planted;
            ShowStateInfo("Семя посажено, требуется полив.");
            hintSpriteRenderer.sprite = hintData.waterSprite;
            IsAvailable = true;
            
            SetSpritesSeedingPoints(GrowStates.Sprout);
            
            Debug.Log(IsAvailable);
        }

        private IEnumerator WaterAndGrow()
        {
            if (seedingScriptableObject == null)
            {
                ShowStateInfo("Нет посаженного растения для полива.");
                IsAvailable = true;
                yield break;
            }

            SetSpritesSeedingPoints(GrowStates.Young);
            
            ShowStateInfo("Полив начат, идет рост...");
            
            yield return new WaitForSeconds(seedingScriptableObject.growTime);
            
            currentState = GardenState.ReadyToHarvest;
            hintSpriteRenderer.sprite = hintData.harvestSprite;
            ShowStateInfo("Рост завершен, готово к сбору.");
            
            SetSpritesSeedingPoints(GrowStates.Mature);
            
            yield return new WaitForSeconds(seedingScriptableObject.growTime);
            
            SetSpritesSeedingPoints(GrowStates.Harvest);

            IsAvailable = true;
        }

        private void Harvest()
        {
            if (seedingScriptableObject != null)
            {
                ClearSpritesFromSeedingPoints();
            }
            
            ShowStateInfo("Урожай собран!");
            currentState = GardenState.Empty;
            hintSpriteRenderer.sprite = null;
            
            IsAvailable = true;
            harvestScriptableObject = null;
        }

        private void SetSpritesSeedingPoints(GrowStates growState)
        {
            for (var i = 0; i < seedingPointsSpriteRenderers.Length; i++)
            {
                seedingPointsSpriteRenderers[i].sprite = seedingScriptableObject.spritesForGarden[(int)growState];
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

