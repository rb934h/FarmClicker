using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Timer {
    
    public event Action OnTimerComplete;

    private float _timeRemaining;
    private bool _isRunning;
    
    private Image _image;

    public Timer(Image image)
    {
        _image = image;
    }
    public void StartTimer(float seconds)
    {
        if (seconds <= 0)
        {
            Debug.LogWarning("Timer duration must be greater than zero.");
            return;
        }

        _timeRemaining = seconds;
        _isRunning = true;
        StartFillAmount(0);
    }

    public void CheckTimer()
    {
        if (!_isRunning) return;
    
        _timeRemaining -= Time.deltaTime;
    
        if (_timeRemaining <= 0)
        {
            StopTimer();
            OnTimerComplete?.Invoke();
        }
    }

    private void StopTimer()
    {
        _isRunning = false;
        _timeRemaining = 0;
    }

    private void StartFillAmount(float amount)
    {
        _image.DOFillAmount(amount, _timeRemaining).SetEase(Ease.Linear).From(1);
    }
}
