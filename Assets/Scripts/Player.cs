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
    [Header("Спрайт игрока")] 
    [SerializeField] private SpriteRenderer playerSprite;
    
    [Header("Настройки движения")] 
    [SerializeField] private float speed = 5f;
    
    [Header("Эмоции")] 
    [SerializeField] private SpriteRenderer playerHint;
    [SerializeField] private PlayerHintData playerHintData;


    private PlayerAnimator _playerAnimator;
    private PointerObject _pointerObject;
    private readonly ActionQueue _actionQueue = new();

    private readonly HashSet<PointerObject> _busyPointerObjects = new ();

    public PlayerInventoryData inventory { get; private set; }
    public PlayerAnimator animator => _playerAnimator;
    
    private IEnumerable<IPointerObjectInteractStrategy> _interactStrategy;

    [Inject]
    public void Construct(PlayerInventoryData playerInventoryData, IEnumerable<IPointerObjectInteractStrategy> interactStrategies)
    {
        inventory = playerInventoryData;
        _interactStrategy = interactStrategies;
    }

    private void Start()
    {
        var animator = GetComponentInChildren<Animator>();
        _playerAnimator = new PlayerAnimator(animator);
    }
    
    public void InteractWithPointerObject<T>(Vector3 targetPosition, T pointerObject)
        where T : PointerObject
    {
        if (_busyPointerObjects.Contains(pointerObject))
            return;
        
        if(Vector3.Distance(transform.position, targetPosition) > 0.1f)
            HintAnimator.Show(pointerObject.SelectedSpriteRenderer);
        
        _actionQueue.Enqueue(async () =>
        {
            _busyPointerObjects.Add(pointerObject);
            
            try
            {
                var flip = transform.position.x > targetPosition.x;
                playerSprite.flipX = flip;
                
                await MoveTo(targetPosition);
                
                HintAnimator.Hide(pointerObject.SelectedSpriteRenderer, false, 0);
                
                foreach (var strategy in _interactStrategy)
                {
                    if (strategy.Interact(this, pointerObject))
                    {
                        await WaitWork(pointerObject.WorkTime);
                        return;
                    }
                       
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
        playerHint.sprite = playerHintData.mistakeSprite;
        HintAnimator.Show(playerHint, 1);
    }
    
    private async UniTask WaitWork(float time)
    {
        int count = playerHintData.workSprites.Length;
        float part = time / count;

        HintAnimator.Show(playerHint, time);

        for (int i = 0; i < count; i++)
        {
            playerHint.sprite = playerHintData.workSprites[i];
            await UniTask.WaitForSeconds(part);
        }
    }

    private async UniTask MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerRun); // OK?🤔
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            await UniTask.Yield();
        }
        
        _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerIdle);
    }
    
    
}