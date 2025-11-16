using System;
using UnityEngine;

namespace Azur.Playable.Carousel
{
    public class CarouselInput
    {
        public event Action MousePressed;
        public event Action MouseReleased;
        public event Action MouseDragging;
        public event Action<float> MouseMovedLeft;
        public event Action<float> MouseMovedRight;

        private readonly float _minMoveDuration;
        private readonly float _maxMoveDuration;
        private readonly CarouselAnimator _carouselAnimator;

        private const float MaxDelta = 25f;

        private bool _previousReversed = true;
        private bool _isDragging;
        private bool _isReversed;

        private Vector3 _lastPosition;
        private Vector2 _delta;

        public CarouselInput(float minMoveDuration, float maxMoveDuration, CarouselAnimator animator)
        {
            _minMoveDuration = minMoveDuration;
            _maxMoveDuration = maxMoveDuration;
            _carouselAnimator = animator;
        }

        public void HandleMouseInput()
        {
            var mousePositionScreen = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                _lastPosition = mousePositionScreen;
                HandleMouseDown();
            }

            if (Input.GetMouseButton(0))
            {
                _delta = (mousePositionScreen - _lastPosition);
                _lastPosition = mousePositionScreen;
                HandleMouseDrag(_delta);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _delta = Vector2.zero;
                HandleMouseUp();
            }
        }

        private void HandleMouseDown()
        {
            MousePressed?.Invoke();
            _isDragging = true;
        }

        private void HandleMouseDrag(Vector2 delta)
        {
            if (!_isDragging || Mathf.Abs(delta.x) <= 1f)
            {
                return;
            }

            MouseDragging?.Invoke();
            HandleDirection(delta.x);
        }

        private void HandleMouseUp()
        {
            MouseReleased?.Invoke();
            _isDragging = false;
        }

        private void HandleDirection(float deltaX)
        {
            _isReversed = deltaX < 0;

            if (_isReversed != _previousReversed)
            {
                MouseMovedLeft?.Invoke(GetMoveDuration(deltaX));
            }
            else
            {
                MouseMovedRight?.Invoke(GetMoveDuration(deltaX));
            }

            _previousReversed = _isReversed;
        }

        private float GetMoveDuration(float deltaX)
        {
            var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(deltaX) / MaxDelta);
            return Mathf.Lerp(
                _maxMoveDuration,
                _minMoveDuration,
                normalizedSpeed
            );
        }
    }
}