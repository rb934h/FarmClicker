using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Enum;
using PointerObjects;
using ScriptableObjects;
using Scripts;
using Strategies;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [Header("Спрайт игрока")] 
        [FormerlySerializedAs("playerSprite")] [SerializeField] private SpriteRenderer _playerSprite;
    
        [Header("Настройки движения")] 
        [FormerlySerializedAs("speed")] [SerializeField] private float _speed = 5f;
        [SerializeField] private AudioSource _stepSound;
    
        [Header("Эмоции")] 
        [FormerlySerializedAs("playerHint")] [SerializeField] private SpriteRenderer _playerHint;
        [FormerlySerializedAs("playerHintData")] [SerializeField] private PlayerHintData _playerHintData;
    
        [Header("Инвентарь")]
        [FormerlySerializedAs("playerInventoryData")] [SerializeField] private PlayerInventoryData _playerInventoryData;
    
        private PlayerAnimator _playerAnimator;
        private PointerObject _pointerObject;
        private ActionQueue _actionQueue;

        private readonly HashSet<PointerObject> _busyPointerObjects = new ();

        public PlayerInventoryData inventory => _playerInventoryData;
        public PlayerAnimator animator => _playerAnimator;
    
        private IEnumerable<IPointerObjectInteractStrategy> _interactStrategy;

        [Inject]
        public void Construct(IEnumerable<IPointerObjectInteractStrategy> interactStrategies)
        {
            _interactStrategy = interactStrategies;
        }

        private void Awake()
        {
            _playerInventoryData = Instantiate(_playerInventoryData);
            _actionQueue = new ActionQueue(this);
        }

        private void Start()
        {
            _playerAnimator = new PlayerAnimator(GetComponentInChildren<Animator>());
        }
    
        public void InteractWithPointerObject<T>(Vector3 targetPosition, T pointerObject)
            where T : PointerObject
        {
            if (_busyPointerObjects.Contains(pointerObject))
                return;

            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                HintAnimator.Show(pointerObject.SelectedSpriteRenderer);

            _actionQueue.Enqueue(async token =>
            {
                _busyPointerObjects.Add(pointerObject);
                bool wasCanceled = false;

                try
                {
                    var flip = transform.position.x > targetPosition.x;
                    transform.rotation = Quaternion.Euler(0, flip ? 180 : 0f, 0);

                    await MoveTo(targetPosition).AttachExternalCancellation(token);

                    HintAnimator.Hide(pointerObject.SelectedSpriteRenderer, false, 0);

                    if (_interactStrategy.Any(s => s.Interact(this, pointerObject)))
                    {
                        await WaitWork(pointerObject.WorkTime)
                            .AttachExternalCancellation(token);

                        return;
                    }

                    MakeMistake();
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("Token was cancelled");
                    wasCanceled = true;
                }
                finally
                {
                    if (!wasCanceled)
                        _busyPointerObjects.Remove(pointerObject);
                }
            });
        }


        private void MakeMistake()
        {
            _playerHint.sprite = _playerHintData.mistakeSprite;
            HintAnimator.Show(_playerHint, 1);
        }
    
        private async UniTask WaitWork(float time)
        {
            var token = this.GetCancellationTokenOnDestroy();
            var count = _playerHintData.workSprites.Length;
            var part = time / count;

            HintAnimator.Show(_playerHint, time);

            for (var i = 0; i < count; i++)
            {
                token.ThrowIfCancellationRequested(); 
                _playerHint.sprite = _playerHintData.workSprites[i];
            
                await UniTask.WaitForSeconds(part, cancellationToken: token);
            }
        }


        private async UniTask MoveTo(Vector2 target)
        {
            var token = this.GetCancellationTokenOnDestroy();
            var wasCanceled = false;
        
            _stepSound.Play();

            try
            {
                while (Vector2.Distance(transform.position, target) > 0.1f)
                {
                    token.ThrowIfCancellationRequested();
                    _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerRun);

                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        target,
                        _speed * Time.deltaTime
                    );

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Token was cancelled");
                wasCanceled = true;
            }
            finally
            {
                if (wasCanceled == false)
                {
                    _stepSound.Stop();
                    _playerAnimator.PlayAnimation(PlayerAnimationState.PlayerIdle); 
                }
            }
        }
    }
}
