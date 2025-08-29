using System;
using System.Collections.Generic;
using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeliveryCar : PointerObject
{
    [SerializeField] private DeliveryCarData deliveryCarData;
    
    [Header("Tile changer")]
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileReplacementRule _rule;
    
    [HideInInspector]
    public DeliveryCarState State = DeliveryCarState.Empty;
    
    public event Action<List<CollectableItemData>> IsSolded;
    
    private TileChanger _tileChanger;
    private List<CollectableItemData> cargo = new ();

    private void Start()
    {
        _tileChanger = new TileChanger(_tilemap, _rule);
    }
    
    public void PutCargo(List<CollectableItemData> objectsFromPlayer)
    {
        cargo.AddRange(objectsFromPlayer);
    }

    public void ClearCargo()
    {
        IsSolded?.Invoke(cargo);
        cargo.Clear();
    }

    public float GetCargoPrice()
    {
        foreach (var collectableItemData in cargo)
        {
            return collectableItemData.price;
        }

        return 0;
    }

    public void Send()
    {
        var defaultPosition = transform.position.x;
        
        transform.DOMoveX(defaultPosition+20, 2f).OnComplete(() =>
        {
            ShowStateInfo("Машина отправлена");
            foreach (var collectableItemData in cargo)
            {
                Debug.Log(collectableItemData.name);
            }
            DOVirtual.DelayedCall(deliveryCarData.deliveryTime,
                () => transform.DOMoveX(defaultPosition, 2f).OnComplete(() =>
                {
                    State = DeliveryCarState.WithMoney;
                    ChangeTile();
                }));
        });
    }
    
    public void ChangeTile()
    {
        _tileChanger.ChangeTiles();
    }
}
