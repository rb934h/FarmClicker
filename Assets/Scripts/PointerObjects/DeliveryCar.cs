using System;
using System.Collections.Generic;
using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;
using VContainer;

public class DeliveryCar : PointerObject
{
    [SerializeField] private DeliveryCarData deliveryCarData;
    
    private DeliveryCarState currentState = DeliveryCarState.Empty;
    private List<CollectableItemData> cargo = new ();
    
    public event Action IsDeparted;
    public event Action IsLoaded;
    public event Action<List<CollectableItemData>> IsSolded;
   
    private PlayerInventoryData _playerInventory;

    [Inject]
    public void Construct(PlayerInventoryData playerInventoryData)
    {
        _playerInventory = playerInventoryData;
    }
    
    public override void ChangeState()
    { 
        
        switch (currentState)
        {
            case DeliveryCarState.Empty:
                if(!_playerInventory.CanAddCargo)
                    return;
                currentState = DeliveryCarState.Loaded;
                ShowStateInfo("Машина загружена");
                PutCargo(_playerInventory.harvestObjects);
                _playerInventory.ClearItems();
                IsLoaded?.Invoke();
                break;
            case DeliveryCarState.Loaded:
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
                            currentState = DeliveryCarState.WithMoney;
                            IsDeparted?.Invoke();
                        }));
                });
                break;
            case DeliveryCarState.WithMoney:
                ShowStateInfo("Забрали деньги");
                IsSolded?.Invoke(cargo);
                cargo.Clear();
                currentState = DeliveryCarState.Empty;
                break;
            default:
                Debug.LogWarning("Unknown DeliveryCar state.");
                break;
        }
        
    }

    private void PutCargo(List<CollectableItemData> objectsFromPlayer)
    {
        cargo.AddRange(objectsFromPlayer);
    }
}
