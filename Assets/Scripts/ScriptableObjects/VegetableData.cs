using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVegetable", menuName = "Vegetable/VegetableData")]
public class VegetableData : CollectableItemData
{
    public Sprite vegetableSprite;
    public GameObject prefab;
    public string itemName;
    public int price;
    public float growTime;
}