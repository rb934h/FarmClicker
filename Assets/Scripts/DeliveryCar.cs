using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;

public class DeliveryCar : PointerObject
{
    [SerializeField] private DeliveryCarData deliveryCarData;
    
    private DeliveryCarState currentState = DeliveryCarState.Empty;
    
    public DeliveryCarState State => currentState;
   
    public override void ChangeState()
    {
        switch (currentState)
        {
            case DeliveryCarState.Empty:
                currentState = DeliveryCarState.Loaded;
                ShowStateInfo("Машина загружена");
                break;
            case DeliveryCarState.Loaded:
                currentState = DeliveryCarState.Departed;
                var defaultPosition = transform.position.x;
                transform.DOMoveX(defaultPosition+20, 2f).OnComplete(() =>
                {
                    ShowStateInfo("Машина отправлена");
                    DOVirtual.DelayedCall(deliveryCarData.deliveryTime,
                        () => transform.DOMoveX(defaultPosition, 2f).OnComplete(() => currentState = DeliveryCarState.WithMoney));
                });
                break;
            case DeliveryCarState.Departed:
                break;
            case DeliveryCarState.WithMoney:
                ShowStateInfo("Забрали деньги");
                currentState = DeliveryCarState.Empty;
                break;
            default:
                Debug.LogWarning("Unknown DeliveryCar state.");
                break;
        }
    }
}
