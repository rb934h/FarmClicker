using System;
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
    [SerializeField] private AudioSource _stepSound;
    
    [Header("Эмоции")] 
    [SerializeField] private SpriteRenderer playerHint;
    [SerializeField] private PlayerHintData playerHintData;
    
    [Header("Инвентарь")]
    [SerializeField] private PlayerInventoryData playerInventoryData;
    
    private PlayerAnimator _playerAnimator;
    private PointerObject _pointerObject;
    private readonly ActionQueue _actionQueue = new();

    private readonly HashSet<PointerObject> _busyPointerObjects = new ();

    public PlayerInventoryData inventory => playerInventoryData;
    public PlayerAnimator animator => _playerAnimator;
    
    private IEnumerable<IPointerObjectInteractStrategy> _interactStrategy;

    [Inject]
    public void Construct(IEnumerable<IPointerObjectInteractStrategy> interactStrategies)
    {
        _interactStrategy = interactStrategies;
    }

    private void Awake()
    {
        playerInventoryData = Instantiate(playerInventoryData);
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
                transform.rotation = Quaternion.Euler(0, flip ? 180 : 0f, 0);
                
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

    private async UniTask MoveTo(Vector2 target)
    {
        _stepSound.Play();
        
        while (Vector2.Distance(transform.position, target) > 0.1f)
        {
            _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerRun); // OK?🤔
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            await UniTask.Yield();
        }
        
        _stepSound.Stop();
        _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerIdle);
    }
    
    
}