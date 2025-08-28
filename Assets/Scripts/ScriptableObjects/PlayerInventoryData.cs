using System;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

[CreateAssetMenu(fileName = "PlayerInventory", menuName = "Game/PlayerInventory")]
public class PlayerInventoryData : ScriptableObject
{
    public CollectableItemData currentSeed;
    public List<CollectableItemData> harvestObjects;
    public bool hasWater;
    [SerializeField] private int maxItemsInHand = 2;
    public float coins = 0;

    public event Action<Sprite> HarvestObjectAdded; 
    public event Action HarvestObjectsClear; 
    public event Action CoinsChanged; 
    public bool CanAddItem => harvestObjects.Count < maxItemsInHand;
    public bool CanAddCargo => harvestObjects.Count > 0;
    
    public void AddHarvestObject(CollectableItemData harvestObject)
    {
        harvestObjects.Add(harvestObject);
        HarvestObjectAdded?.Invoke(harvestObject.spriteForHands);
    }

    public void UseWater() => hasWater = false;
    public void FillWater() => hasWater = true;
    public void AddCoins(float coins)
    {
        this.coins += coins;
        CoinsChanged?.Invoke();
    }

    public void ClearItems()
    {
        harvestObjects.Clear();
        HarvestObjectsClear?.Invoke();
    }
}