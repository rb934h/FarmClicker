using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "Plant/PlnatData")]
public class PlantData : ScriptableObject
{
    public GameObject prefab;
    public string name;
    public int price;
    public float growTime;
}