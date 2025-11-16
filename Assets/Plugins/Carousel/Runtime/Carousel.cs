using System.Linq;
using UnityEngine;

namespace Azur.Playable.Carousel
{
    public class Carousel : MonoBehaviour
    {
        [Header("Items position parameters")] 
        [SerializeField] private float _itemDepth = 5;
        [SerializeField] private float _itemSpacing = 5;

        [Space] 
        [Header("Input parameters")] 
        [SerializeField] private float _minMoveDuration = .2f;
        [SerializeField] private float _maxMoveDuration = .5f;

        [Space] 
        [Header("Scroll parameters")] 
        [SerializeField] private float _returnDelay = 0;
        [Range(0, 1)] 
        [SerializeField] private float _returnThreshold = .25f;

        [Header("Auto-scroll parameters")] 
        [SerializeField] private bool _autoScroll = true;
        [SerializeField] private float _idleThreshold = 1f;
        [SerializeField] private float _autoScrollMoveDuration = 1f;
        [Space] 
        [SerializeField] private CarouselEase _easeType;

        private CarouselInput _input;
        private CarouselAnimator _animator;
        private ItemDataAccess _itemDataAccess;
        private CarouselUIService _uiService;

        private void Subscribe()
        {
            _animator.SequenceUpdated += _uiService.RequestSortingOrderUpdate;

            _input.MousePressed += _animator.ResetIdleTime;
            _input.MouseDragging += _animator.ResetIdleTime;

            _input.MouseMovedRight += OnMouseMovedRight;
            _input.MouseMovedLeft += OnMouseMovedLeft;

            _input.MouseReleased += _animator.Pause;
            _input.MouseReleased += _animator.Rollback;
        }
        
        private void Start()
        {
            Initialize();
            Subscribe();
        }

        private void Update()
        {
            _animator.PlayAutoScroll(Time.deltaTime);
            _input.HandleMouseInput();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            Initialize();
        }

        private void Initialize()
        {
            var carouselChildren = transform.Cast<Transform>().ToList();
            _itemDataAccess = new ItemDataAccess(_itemDepth, _itemSpacing, carouselChildren);

            var isCanvas = TryGetComponent<RectTransform>(out _);
            _uiService = new CarouselUIService(isCanvas, carouselChildren);

            var animatorConfig = new CarouselAnimatorConfig
            {
                ReturnDelay = _returnDelay,
                ReturnThreshold = _returnThreshold,
                AutoScroll = _autoScroll,
                IdleThreshold = _idleThreshold,
                AutoScrollMoveDuration = _autoScrollMoveDuration,
                EaseType = _easeType,
            };
            _animator ??= new CarouselAnimator(_itemDataAccess, _uiService, animatorConfig);

            _input ??= new CarouselInput(_minMoveDuration, _maxMoveDuration, _animator);
        }
        
        private void OnMouseMovedLeft(float duration)
        {
            _animator.PlayBackward(duration);
        }

        private void OnMouseMovedRight(float duration)
        {
            _animator.PlayForward(duration);
        }
        
        private void UnSubscribe()
        {
            _animator.SequenceUpdated -= _uiService.RequestSortingOrderUpdate;

            _input.MousePressed -= _animator.ResetIdleTime;
            _input.MouseDragging -= _animator.ResetIdleTime;

            _input.MouseMovedRight -= OnMouseMovedRight;
            _input.MouseMovedLeft -= OnMouseMovedLeft;

            _input.MouseReleased -= _animator.Pause;
            _input.MouseReleased -= _animator.Rollback;
        }
        
        private void OnDisable()
        {
            UnSubscribe();
        }
    }
}