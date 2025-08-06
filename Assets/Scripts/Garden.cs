using System.Collections;
using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;


    public class Garden : PointerObject
    {
        [SerializeField] private MeshRenderer gardenRenderer;
        [SerializeField] private SpriteRenderer hintSpriteRenderer;
        [SerializeField] private HintData hintData;

        private ScriptableObject seedingScriptableObject; 
         
        private Material[] gardenMaterials;
        private GameObject seedingPrefab; 
        private GameObject currentSeedingInstance; 
        private GardenState currentState = GardenState.Empty;
        private Color gardenColorAfterWaterSeed;
        private Color gardenDefaultColor;
        private string seedingName; 
        private float seedingGrowTime; 
        
        public GardenState State => currentState;
        

        private void Start()
        {
            gardenMaterials = gardenRenderer.materials;
            gardenDefaultColor = gardenMaterials[1].color;
            gardenColorAfterWaterSeed = new Color(0.7f, 0.5f, 0.35f);
        }

        public override void ChangeState()
        {
            switch (currentState)
            {
                case GardenState.Empty:
                    StartCoroutine(PlantSeed());
                    break;
                case GardenState.Planted:
                    WaterSeed();
                    break;
                case GardenState.ReadyToHarvest:
                    Harvest();
                    break;
                default:
                    Debug.Log("Нет доступного действия для текущего состояния: " + currentState);
                    break;
            }
            
        }
        
        public void SetSeedingData(ScriptableObject seedingData)
        {
            seedingScriptableObject = seedingData;

            switch (seedingScriptableObject)
            {
                case PlantData plantData:
                    ApplySeedingData(plantData.name, plantData.prefab, plantData.growTime);
                    break;
                case VegetableData vegetableData:
                    ApplySeedingData(vegetableData.name, vegetableData.prefab, vegetableData.growTime);
                    break;
                default:
                    Debug.LogWarning("Неизвестный тип данных для посадки.");
                    break;
            }
        }
        
        private void ApplySeedingData(string name, GameObject prefab, float growTime)
        {
            seedingName = name;
            seedingPrefab = prefab;
            seedingGrowTime = growTime;
        }
        
        private IEnumerator PlantSeed()
        {
            if (seedingPrefab == null)
            {
                Debug.LogWarning("Невозможно посадить: нет данных о семени.");
                yield break;
            }
            
            Vector3 spawnPosition = new Vector3(transform.position.x, 0, transform.position.z);
            currentSeedingInstance = Instantiate(seedingPrefab, spawnPosition, Quaternion.identity);
            
            yield return new WaitForSeconds(seedingGrowTime);
            
            currentState = GardenState.Planted;
            Debug.Log("Семя посажено, требуется полив.");
            hintSpriteRenderer.sprite = hintData.waterSprite;
        }

        private void WaterSeed()
        {
            if (currentSeedingInstance == null)
            {
                Debug.LogWarning("Нет посаженного растения для полива.");
                return;
            }

            foreach (var gardenMaterial in gardenMaterials)
            {
                gardenMaterial.DOColor(gardenColorAfterWaterSeed, seedingGrowTime);
            }
            
            Debug.Log("Полив начат, идет рост...");
            currentState = GardenState.Watered;
            StartCoroutine(GrowCoroutine());
        }

        private IEnumerator GrowCoroutine()
        {
            yield return new WaitForSeconds(seedingGrowTime);

            Debug.Log("Рост завершен, готово к сбору.");
            currentState = GardenState.ReadyToHarvest;
            hintSpriteRenderer.sprite = hintData.harvestSprite;
        }

        private void Harvest()
        {
            if (currentSeedingInstance != null)
            {
                Destroy(currentSeedingInstance);
            }

            Debug.Log("Урожай собран!");
            currentState = GardenState.Empty;
            hintSpriteRenderer.sprite = null;
            
            foreach (var gardenMaterial in gardenMaterials)
            {
                gardenMaterial.DOColor(gardenDefaultColor, seedingGrowTime);
            }
        }
    }

