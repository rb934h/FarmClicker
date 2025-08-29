using System;
using Enum;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTank : PointerObject
{
    [Header("Tile changer")]
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileReplacementRule _rule;
    
    [HideInInspector]
    public WaterTankState State = WaterTankState.Empty;
    
    private TileChanger _tileChanger;

    private void Start()
    {
        _tileChanger = new TileChanger(_tilemap, _rule);
    }
    
    public void ChangeTile()
    {
        _tileChanger.ChangeTiles();
    }
}
