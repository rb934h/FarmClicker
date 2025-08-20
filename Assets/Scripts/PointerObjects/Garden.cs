using System.Collections;
using System.Collections.Generic;
using Enum;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Garden : PointerObject
    {
        //[SerializeField] private List<Tile> gardenTiles;
        [SerializeField] private SpriteRenderer hintSpriteRenderer;
        [SerializeField] private HintData hintData;

        private CollectableItemData seedingScriptableObject; 
        private CollectableItemData harvestScriptableObject; 
        

        private GameObject seedingPrefab; 
        private GameObject currentSeedingInstance; 
        private GardenState currentState = GardenState.Empty;
        private Color gardenColorAfterWaterSeed;
        private Color gardenDefaultColor;
        private string seedingName; 
        private Sprite seedingSprite; 
        private float seedingGrowTime; 
        private float seedingPrice; 
      
        
        public GameObject SeedingPrefab => seedingPrefab;
        public Sprite SeedingSprite => seedingSprite;
        public float SeedingPrice => seedingPrice;
        public GardenState State => currentState;


        public CollectableItemData GetHarvestObject()
        {
            return harvestScriptableObject;
        }
        private void Start()
        {
           // gardenDefaultColor = gardenTiles[0].color;
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

            switch (seedingScriptableObject)
            {
                case PlantData plantData:
                    ApplySeedingData(plantData.name, plantData.prefab, plantData.growTime, plantData.price, plantData.plantSprite);
                    break;
                case VegetableData vegetableData:
                    ApplySeedingData(vegetableData.name, vegetableData.prefab, vegetableData.growTime, vegetableData.price, vegetableData.vegetableSprite);
                    break;
                default:
                    Debug.LogWarning("Неизвестный тип данных для посадки.");
                    break;
            }
        }
        
        private void ApplySeedingData(string name, GameObject prefab, float growTime, float price, Sprite itemSprite)
        {
            seedingSprite = itemSprite;
            seedingName = name;
            seedingPrefab = prefab;
            seedingGrowTime = growTime;
            seedingPrice = price;
        }
        
        private IEnumerator PlantSeed()
        {
            if (seedingPrefab == null)
            {
                ShowStateInfo("Невозможно посадить: нет данных о семени.");
                IsAvailable = true;
                yield break;
            }
            
            harvestScriptableObject = seedingScriptableObject;
            
            Vector3 spawnPosition = new Vector3(transform.position.x, 0, transform.position.z);
            currentSeedingInstance = Instantiate(seedingPrefab, spawnPosition, Quaternion.identity);
            
            yield return new WaitForSeconds(seedingGrowTime);
            
            currentState = GardenState.Planted;
            ShowStateInfo("Семя посажено, требуется полив.");
            hintSpriteRenderer.sprite = hintData.waterSprite;
            IsAvailable = true;
            
            Debug.Log(IsAvailable);
        }

        private IEnumerator WaterAndGrow()
        {
            if (currentSeedingInstance == null)
            {
                ShowStateInfo("Нет посаженного растения для полива.");
                IsAvailable = true;
                yield break;
            }

            ShowStateInfo("Полив начат, идет рост...");
            
            // foreach (var gardenTile in gardenTiles)
            // {
            //     gardenTile.color = gardenColorAfterWaterSeed;
            // }
            
            yield return new WaitForSeconds(seedingGrowTime);
            
            currentState = GardenState.ReadyToHarvest;
            hintSpriteRenderer.sprite = hintData.harvestSprite;
            ShowStateInfo("Рост завершен, готово к сбору.");

            IsAvailable = true;
        }

        private void Harvest()
        {
            if (currentSeedingInstance != null)
            {
                Destroy(currentSeedingInstance);
            }
            
            ShowStateInfo("Урожай собран!");
            currentState = GardenState.Empty;
            hintSpriteRenderer.sprite = null;
            
            // foreach (var gardenTile in gardenTiles)
            // {
            //     gardenTile.color = gardenDefaultColor;
            // }

            IsAvailable = true;
            harvestScriptableObject = null;
        }
    }

