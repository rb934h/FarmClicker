using System.Collections;
using Enum;
using ScriptableObjects;
using UnityEngine;
using VContainer;

public class Garden : PointerObject
    {
        [SerializeField] private SpriteRenderer[] seedingPointsSpriteRenderers;
        [SerializeField] private SpriteRenderer hintSpriteRenderer;
        [SerializeField] private GardenHintData hintData;

        private CollectableItemData seedingScriptableObject; 
        private CollectableItemData harvestScriptableObject; 
        
        
        private GardenState currentState = GardenState.Empty;
        
        public CollectableItemData SeedingScriptableObject => seedingScriptableObject;
        public GardenState State => currentState;
        
        private PlayerInventoryData _playerInventory;

        [Inject]
        public void Construct(PlayerInventoryData playerInventoryData)
        {
            _playerInventory = playerInventoryData;
        }
        
        public CollectableItemData GetHarvestObject()
        {
            return harvestScriptableObject;
        }
        
        // public override void ChangeState()
        // {
        //     switch (State)
        //     {
        //         case GardenState.Empty:
        //             StartCoroutine(PlantSeed());
        //             break;
        //         case GardenState.Planted:
        //             if (!_playerInventory.hasWater)
        //             {
        //                 Debug.LogWarning("Нет воды для полива");
        //                 return;
        //             }
        //             _playerInventory.UseWater();
        //             StartCoroutine(WaterAndGrow());
        //             OnPlayerAnimationStateChanged(PlayerAnimationState.PlayerWatering);
        //             break;
        //
        //         case GardenState.ReadyToHarvest:
        //             if (!_playerInventory.CanAddItem)
        //             {
        //                 Debug.LogWarning("Руки уже заняты");
        //                 return;
        //             }
        //             
        //             _playerInventory.AddHarvestObject(harvestScriptableObject);
        //             
        //             Harvest();
        //             OnPlayerAnimationStateChanged(PlayerAnimationState.PlayerWeeding);
        //             
        //             break;
        //     }
        // }
        
        public void SetSeedingData(CollectableItemData seedingData)
        {
            if(harvestScriptableObject == null)
                seedingScriptableObject = seedingData;
        }
        
        public IEnumerator PlantSeed()
        {
            if (seedingScriptableObject == null)
            {
                ShowStateInfo("Невозможно посадить: нет данных о семени.");
                IsAvailable = true;
                yield break;
            }
            
            harvestScriptableObject = seedingScriptableObject;

            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Seed));
            
            yield return new WaitForSeconds(seedingScriptableObject.growTime);
            
            currentState = GardenState.Planted;
            ShowStateInfo("Семя посажено, требуется полив.");
            hintSpriteRenderer.sprite = hintData.waterSprite;
            IsAvailable = true;
            
            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Sprout));
            
            Debug.Log(IsAvailable);
        }

        public IEnumerator WaterAndGrow()
        {
            if (seedingScriptableObject == null)
            {
                ShowStateInfo("Нет посаженного растения для полива.");
                IsAvailable = true;
                yield break;
            }

            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Young));
            
            ShowStateInfo("Полив начат, идет рост...");
            
            yield return new WaitForSeconds(seedingScriptableObject.growTime);
            
            currentState = GardenState.ReadyToHarvest;
            hintSpriteRenderer.sprite = hintData.harvestSprite;
            
            ShowStateInfo("Рост завершен, готово к сбору.");
            
            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Mature));
            
            yield return new WaitForSeconds(seedingScriptableObject.growTime);
            
            StartCoroutine(SetSpritesSeedingPoints(GrowStates.Harvest));

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

        private IEnumerator SetSpritesSeedingPoints(GrowStates growState)
        {
            for (var i = 0; i < seedingPointsSpriteRenderers.Length; i++)
            {
                yield return new WaitForSeconds(.05f);
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

