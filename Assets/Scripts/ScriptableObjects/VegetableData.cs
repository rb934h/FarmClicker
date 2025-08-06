using UnityEngine;

[CreateAssetMenu(fileName = "NewVegetable", menuName = "Vegetable/VegetableData")]
public class VegetableData : ScriptableObject
{
    public GameObject prefab;
    public string name;
    public int price;
    public float growTime;
}