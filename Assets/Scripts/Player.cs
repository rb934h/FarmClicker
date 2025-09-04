using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Enum;
using PointerObjects;
using ScriptableObjects;
using Scripts;
using Strategies;
using VContainer;

public class Player : MonoBehaviour
{
    [Header("Настройки движения")] 
    [SerializeField] private float speed = 5f;
    
    [Header("Эмоции")] 
    [SerializeField] private SpriteRenderer _playerHint;
    [SerializeField] private PlayerHintData _playerHintData;


    private PlayerAnimator _playerAnimator;
    private PointerObject _pointerObject;
    private readonly ActionQueue _actionQueue = new();

    private readonly HashSet<PointerObject> _busyPointerObjects = new ();

    public PlayerInventoryData Inventory { get; private set; }
    public PlayerAnimator Animator => _playerAnimator;
    
    private IEnumerable<IPointerObjectInteractStrategy> _interactStrategy;

    [Inject]
    public void Construct(PlayerInventoryData playerInventoryData, IEnumerable<IPointerObjectInteractStrategy> interactStrategies)
    {
        Inventory = playerInventoryData;
        _interactStrategy = interactStrategies;
    }

    private void Start()
    {
        var animator = GetComponentInChildren<Animator>();
        _playerAnimator = new PlayerAnimator(animator);
    }
    

    public void SetSeedingData(CollectableItemData seedingData)
    {
        Inventory.currentSeed = seedingData;
    }
    
    public void InteractWithPointerObject<T>(Vector3 targetPosition, T pointerObject)
        where T : PointerObject
    {
        if (_busyPointerObjects.Contains(pointerObject))
            return;
        
        
        _actionQueue.Enqueue(async () =>
        {
            _busyPointerObjects.Add(pointerObject);
            try
            {
                var yRotation = transform.position.x >= targetPosition.x ? 0f : -180f;
                
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z);
                
                await MoveTo(targetPosition);
                
                foreach (var strategy in _interactStrategy)
                {
                    if(strategy.Interact(this, pointerObject))
                        return;
                }
                
                MakeMistake();
            }
            finally
            {
                _busyPointerObjects.Remove(pointerObject);
            }
        });
        
        
    }

    private void MakeMistake()
    {
        _playerHint.sprite = _playerHintData.mistakeSprite;
        HintAnimator.Show(_playerHint, true);
    }

    private async UniTask MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerRun); // ITS OK?🤔
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            await UniTask.Yield();
        }
        
        _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerIdle);
    }
}