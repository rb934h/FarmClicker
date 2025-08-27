using System;
using System.Collections.Generic;
using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;

public class DeliveryCar : PointerObject
{
    [SerializeField] private DeliveryCarData deliveryCarData;
    
    private DeliveryCarState currentState = DeliveryCarState.Empty;
    private List<CollectableItemData> cargo = new ();
    
    public DeliveryCarState State => currentState;
    
    public event Action<List<CollectableItemData>> IsDeparted;
    public event Action IsLoaded;
    public event Action IsSolded;
   
    public override void ChangeState()
    { 
        if(!IsAvailable)
            return;
            
        IsAvailable = false;
        
        switch (currentState)
        {
            case DeliveryCarState.Empty:
                currentState = DeliveryCarState.Loaded;
                ShowStateInfo("Машина загружена");
                IsLoaded?.Invoke();
                break;
            case DeliveryCarState.Loaded:
                var defaultPosition = transform.position.x;
                transform.DOMoveX(defaultPosition+20, 2f).OnComplete(() =>
                {
                    ShowStateInfo("Машина отправлена");
                    IsDeparted?.Invoke(cargo);
                    foreach (var collectableItemData in cargo)
                    {
                        Debug.Log(collectableItemData.name);
                    }
                    cargo.Clear();
                    DOVirtual.DelayedCall(deliveryCarData.deliveryTime,
                        () => transform.DOMoveX(defaultPosition, 2f).OnComplete(() =>
                        {
                            currentState = DeliveryCarState.WithMoney;
                            IsSolded?.Invoke();
                        }));
                });
                break;
            case DeliveryCarState.WithMoney:
                ShowStateInfo("Забрали деньги");
                currentState = DeliveryCarState.Empty;
                break;
            default:
                Debug.LogWarning("Unknown DeliveryCar state.");
                break;
        }
        
        IsAvailable = true;
    }

    public void PutCargo(CollectableItemData objectFromPlayer)
    {
        cargo.Add(objectFromPlayer);
    }
}
