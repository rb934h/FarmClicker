using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Enum; 

public class Player : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float diggingTime = 5f;

    private ScriptableObject _seedingData;
    private PointerObject _pointerObject;
    private CharacterState _state = CharacterState.Idle;
    private readonly ActionQueue _actionQueue = new();
    
    public event Action <PointerObject> OnDig;
    public event Action <PointerObject> DigEnded;
    public event Action <PointerObject> OnWaterTankInteract;
    public event Action <PointerObject> WaterTankInteractionEnded;
    
    public bool IsHaveWater = false;
    
    public ScriptableObject SeedingData
    {
        get => _seedingData;
    }
    
    public void StartDigging(Vector3 positionForDigging, Garden garden)
    {
        _actionQueue.Enqueue(async () =>
        {
            if (garden.State == GardenState.Planted)
            {
                if (!IsHaveWater)
                {
                    Debug.LogWarning("Нельзя копать, пока не полили семена!");
                    return;
                } 
                 
                IsHaveWater = false;
            } 
            
            var currentPointerObject = garden;
            await DigSequence(positionForDigging, currentPointerObject);
        });
    }
    public void SetSeedingData(ScriptableObject seedingData)
    {
        _seedingData = seedingData;
    }
    
    public void InteractWithWaterTank(Vector3 positionForGetWater, WaterTank waterTank)
    {
        _actionQueue.Enqueue(async () =>
        {
            if (waterTank.State == WaterTankState.ReadyToCollect)
            {
                IsHaveWater = true;
            }
            
            var currentPointerObject = waterTank;
            await WaterTankSequence(positionForGetWater, currentPointerObject);
        });
    }
    
    private async UniTask WaterTankSequence(Vector3 positionForInteract, PointerObject currentPointerObject)
    {
        SetState(CharacterState.Walking);
        await MoveTo(positionForInteract);

        SetState(CharacterState.Digging);
        OnWaterTankInteract?.Invoke(currentPointerObject);
        await UniTask.WaitForSeconds(diggingTime);
        WaterTankInteractionEnded?.Invoke(currentPointerObject);

        SetState(CharacterState.Idle);
    }
    private async UniTask DigSequence(Vector3 positionForDigging, PointerObject currentPointerObject)
    {
        SetState(CharacterState.Walking);
        await MoveTo(positionForDigging);

        SetState(CharacterState.Digging);
        OnDig?.Invoke(currentPointerObject);
        await UniTask.WaitForSeconds(diggingTime);
        DigEnded?.Invoke(currentPointerObject);

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
        Debug.Log($"Состояние: {_state}");
    } 
}