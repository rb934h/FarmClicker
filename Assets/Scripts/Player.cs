using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using Enum;
using ScriptableObjects;

public class Player : MonoBehaviour
{
    [Header("Настройки движения")] 
    [SerializeField] private float speed = 5f;
    [SerializeField] private float workTime = 5f;
    
    [Header("UI")] 
    [SerializeField] private PlayerWallet playerWallet;
    
    [Header("Анимация")] 
    [SerializeField] private PlayerAnimator _playerAnimator;
    

    private CollectableItemData _seedingData;
    private PointerObject _pointerObject;
    private CharacterState _state = CharacterState.Idle;
    private CollectableItemData _harvestedScriptableObject;
    private readonly ActionQueue _actionQueue = new();
    private float _seedingPrice;
    

    private readonly HashSet<PointerObject> _busyPointerObjects = new ();


    public event Action<PointerObject> MoveCompleted;
    public event Action<PointerObject> WorkCompleted;

    public bool isHaveWater;

    public CollectableItemData SeedingData
    {
        get => _seedingData;
    }

    public float CheckWallet()
    {
        return playerWallet.GetMoney();
    }

    public void SetSeedingData(CollectableItemData seedingData)
    {
        _seedingData = seedingData;
    }

    public void InteractWithGarden(Vector3 pos, Garden garden)
    {
        InteractWithPointerObject(pos, garden, async _ => { await UniTask.WaitForSeconds(workTime); });
    }

    public void InteractWithWaterTank(Vector3 pos, WaterTank tank)
    {
        InteractWithPointerObject(pos, tank, async t =>
        {
            if (!isHaveWater && t.State == WaterTankState.ReadyToCollect)
                isHaveWater = true;
            await UniTask.WaitForSeconds(workTime);
        });
    }

    public void InteractWithDeliveryCar(Vector3 pos, DeliveryCar car)
    {
        InteractWithPointerObject(pos, car, async c =>
        {
            if (c.State == DeliveryCarState.Empty)
                PutCargoToCar(c, _harvestedScriptableObject);

            if (c.State == DeliveryCarState.WithMoney)
                playerWallet.SetMoney(_seedingPrice);
            
            await UniTask.WaitForSeconds(workTime);
        });
    }
    
    private void InteractWithPointerObject<T>(Vector3 targetPosition, T pointerObject, Func<T, UniTask> work)
        where T : PointerObject
    {
        if (_busyPointerObjects.Contains(pointerObject))
            return;

        _actionQueue.Enqueue(async () =>
        {
            _busyPointerObjects.Add(pointerObject);
            try
            {
                float yRotation = transform.position.x >= targetPosition.x ? 0f : -180f;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);
                
                _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerRun);
                await MoveTo(targetPosition);
                MoveCompleted?.Invoke(pointerObject);
                _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerIdle);
                
                switch (pointerObject)
                {
                    case Garden garden:
                        if (garden.State == GardenState.Planted)
                        {
                            if (!isHaveWater)
                            {
                                Debug.LogWarning("Нет воды для полива");
                                return;
                            }
                            _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerWatering);
                            isHaveWater = false;
                        }
                        else if (garden.State == GardenState.ReadyToHarvest)
                        {
                            if (_harvestedScriptableObject != null)
                            {
                                Debug.LogWarning("Руки уже заняты");
                                return;
                            }
                            _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerWeeding);
                            _seedingPrice = garden.SeedingPrice;
                            _harvestedScriptableObject = garden.GetHarvestObject();
                        }
                        break;

                    case DeliveryCar car:
                        if (car.State == DeliveryCarState.Empty && _harvestedScriptableObject == null)
                        {
                            Debug.LogWarning("Нужно что-то положить в машину");
                            return;
                        }
                        break;
                    
                    case WaterTank waterTank:
                        if (waterTank.State == WaterTankState.ReadyToCollect && isHaveWater)
                        {
                            Debug.LogWarning("В руках уже есть вода");
                            return;
                        }
                        break;
                }

                await work(pointerObject);

                WorkCompleted?.Invoke(pointerObject);
            }
            finally
            {
                _busyPointerObjects.Remove(pointerObject);
                SetState(CharacterState.Idle);
            }
        });
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