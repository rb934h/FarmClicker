using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "Plant/PlnatData")]
public class PlantData : CollectableItemData
{
    public Sprite plantSprite;
    public GameObject prefab;
    public string itemName;
    public int price;
    public float growTime;
}