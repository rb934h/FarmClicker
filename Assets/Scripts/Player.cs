using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Enum;
using ScriptableObjects;

public class Player : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float diggingTime = 5f;
    
    [Header("UI")]
    [SerializeField] private PlayerWallet _playerWallet;

    private CollectableItemData _seedingData;
    private PointerObject _pointerObject;
    private CharacterState _state = CharacterState.Idle;
    private CollectableItemData _harvestedScriptableObject;
    private readonly ActionQueue _actionQueue = new();
    private float _seedingPrice;

    
    public event Action <PointerObject> Interacted;
    public event Action <PointerObject> InteractEnded;
    
    public bool IsHaveWater ;
    public bool IsHavePackage ;
    

    public CollectableItemData SeedingData
    {
        get => _seedingData;
    }

    public float CheckWallet()
    {
        return _playerWallet.GetMoney();
    }
    
    public void SetSeedingData(CollectableItemData seedingData)
    {
        _seedingData = seedingData;
    }
    
    public void InteractWithGarden(Vector3 positionForDigging, Garden garden)
    {
        _actionQueue.Enqueue(async () =>
        {
            var currentPointerObject = garden;
            
            switch (garden.State)
            {
                case GardenState.Planted:
                    if (!IsHaveWater)
                    {
                        Debug.LogWarning("Нужна вода, чтобы полить");
                        return;
                    } 
                    IsHaveWater = false;
                    break;
                case GardenState.ReadyToHarvest:
                    if (_harvestedScriptableObject != null)
                    {
                        Debug.LogWarning("Руки уже заняты");
                        return;
                    }

                    _seedingPrice = garden.SeedingPrice;
                    _harvestedScriptableObject = garden.GetSeedingObject();
                    break;
            }
            
            await InteractSequence(positionForDigging, currentPointerObject);
        });
    }
    public void InteractWithWaterTank(Vector3 positionForGetWater, WaterTank waterTank)
    {
        _actionQueue.Enqueue(async () =>
        {
            if(IsHaveWater)
                return;
            
            if (waterTank.State == WaterTankState.ReadyToCollect)
            {
                IsHaveWater = true;
            }
            
            var currentPointerObject = waterTank;
            await InteractSequence(positionForGetWater, currentPointerObject);
        });
    }
    
    public void InteractWithDeliveryCar(Vector3 positionForGetWater, DeliveryCar deliveryCar)
    {
        _actionQueue.Enqueue(async () =>
        {
            if (deliveryCar.State == DeliveryCarState.Empty )
            {
                if (_harvestedScriptableObject == null)
                {
                    Debug.LogWarning("Нужно что-то положить в машину");
                    return;
                }

                PutCargoToCar(deliveryCar,_harvestedScriptableObject);
            }
            
            if (deliveryCar.State == DeliveryCarState.WithMoney )
            {
                _playerWallet.SetMoney(_seedingPrice);
            }
            
            var currentPointerObject = deliveryCar;
            await InteractSequence(positionForGetWater, currentPointerObject);
        });
    }
    
    private async UniTask InteractSequence(Vector3 positionForDigging, PointerObject currentPointerObject)
    {
        SetState(CharacterState.Walking);
        await MoveTo(positionForDigging);
        SetState(CharacterState.Digging);
        Interacted?.Invoke(currentPointerObject);
        await UniTask.WaitForSeconds(diggingTime);
        InteractEnded?.Invoke(currentPointerObject);

        SetState(CharacterState.Idle);
    }
    
    private async UniTask MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }
    
    private void SetState(CharacterState newState)
    {
        _state = newState;
    }

    private void PutCargoToCar(DeliveryCar deliveryCar, CollectableItemData cargo)
    {
        deliveryCar.PutCargo(cargo);
        _harvestedScriptableObject = null;
    }
}