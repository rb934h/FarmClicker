using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Enum;
using ScriptableObjects;
using VContainer;

public class Player : MonoBehaviour
{
    [Header("Настройки движения")] 
    [SerializeField] private float speed = 5f;
    [SerializeField] private float workTime = 5f;
    
    
    [Header("Анимация")] 
    [SerializeField] private PlayerAnimator _playerAnimator;
    
    private PointerObject _pointerObject;
    private readonly ActionQueue _actionQueue = new();

    private readonly HashSet<PointerObject> _busyPointerObjects = new ();
    
    public event Action<PointerObject> WorkCompleted;

    private PlayerInventoryData _playerInventory;

    [Inject]
    public void Construct(PlayerInventoryData playerInventoryData)
    {
        _playerInventory = playerInventoryData;
    }
    
    public CollectableItemData SeedingData
    {
        get => _playerInventory.currentSeed;
    }

    public void SetSeedingData(CollectableItemData seedingData)
    {
        _playerInventory.currentSeed = seedingData;
    }
    
    public void InteractWithPointerObject<T>(Vector3 targetPosition, T pointerObject,  Func<T, UniTask> work)
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

                _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerIdle);

                pointerObject.PlayerAnimationStateChanged += _playerAnimator.PlayAnimation;

                await work(pointerObject);
                
                WorkCompleted?.Invoke(pointerObject);
            }
            finally
            {
                _busyPointerObjects.Remove(pointerObject);
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
}