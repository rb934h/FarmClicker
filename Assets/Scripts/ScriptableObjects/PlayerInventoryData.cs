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

    public event Action<CollectableItemData> HarvestObjectAdded; 
    public event Action<CollectableItemData> HarvestObjectsClear; 
    public event Action OnWaterFilled; 
    public event Action OnWaterUsed; 
    public event Action CoinsChanged; 
    public bool canAddItem => harvestObjects.Count < maxItemsInHand;
    public bool handsNotEmpty => harvestObjects.Count > 0;
    
    public void AddHarvestObject(CollectableItemData harvestObject)
    {
        harvestObjects.Add(harvestObject);
        HarvestObjectAdded?.Invoke(harvestObject);
    }

    public void UseWater()
    {
        hasWater = false;
        maxItemsInHand++;
        OnWaterUsed?.Invoke();
    }

    public void FillWater()
    {
        hasWater = true;
        maxItemsInHand--;
        OnWaterFilled?.Invoke();
    }

    public void AddCoins(float coinsCount)
    {
        coins += coinsCount;
        CoinsChanged?.Invoke();
    }

    public void ClearItem(CollectableItemData item)
    {
        harvestObjects.Remove(item);
        HarvestObjectsClear?.Invoke(item);
    }
}