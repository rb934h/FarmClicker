using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private List<Image> _images = new();
    [SerializeField] private TMP_Text _timeText;
    
    private enum Period { AM, PM }
    private Period _previousPeriod = Period.AM;
    
    private float _timeRemaining;
    private bool _isRunning;
    private Sequence _sequence;
    
    public event Action OnTimerComplete;
    public event Action DayPeriodChanged;
    public event Action EveningArrived;

    public void StartTimer(float seconds)
    {
        if (seconds <= 0)
        {
            Debug.LogWarning("Timer duration must be greater than zero.");
            return;
        }

        if (_images.Count < 4)
        {
            Debug.LogWarning("Timer requires 4 images.");
            return;
        }

        _timeRemaining = seconds;
        _isRunning = true;
        PlayFadeSequence();
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

    private void PlayFadeSequence()
    {
        _sequence?.Kill();

        _isRunning = true;
        InitializeImageAlphas();

        var segmentDuration = _timeRemaining / (_images.Count - 1);
        _sequence = DOTween.Sequence();
        
        for (var i = 0; i < _images.Count - 1; i++)
        {
            var startTime = i * segmentDuration;

            _sequence.Insert(startTime, _images[i].DOFade(0f, segmentDuration).SetEase(Ease.Linear));
            _sequence.Insert(startTime, _images[i + 1].DOFade(1f, segmentDuration).SetEase(Ease.Linear));
        }

        
        var startMinutes = 10f * 60f;
        var endMinutes = 20f * 60f;
        var currentMinutes = startMinutes;

        Sequence timeTween = DOTween.Sequence();
        timeTween.Append(DOTween.To(() => currentMinutes, x =>
        {
            currentMinutes = x;
            UpdateTimeText(currentMinutes);
        }, endMinutes, _timeRemaining).SetEase(Ease.Linear));

        _sequence.Insert(0f, timeTween); 

        _sequence.OnComplete(() =>
        {
            _isRunning = false;
            _timeRemaining = 0f;
            OnTimerComplete?.Invoke();
        });

        _sequence.Play();
    }

    private void InitializeImageAlphas()
    {
        for (var i = 0; i < _images.Count; i++)
        {
            var color = _images[i].color;
            color.a = (i == 0) ? 1f : 0f;
            _images[i].color = color;
        }
    }
    
    private void UpdateTimeText(float totalMinutes)
    {
        var hours = Mathf.FloorToInt(totalMinutes / 60f);
        var currentPeriod = hours >= 12 ? Period.PM : Period.AM;

        if (currentPeriod != _previousPeriod)
        {
           DayPeriodChanged?.Invoke();
        }

        _previousPeriod = currentPeriod;
        
        var eveningHour = 16;
        var displayHours = hours % 12; 
        
        if (displayHours == 0) 
            displayHours = 12; 
        _timeText.text = $"{displayHours:00} {currentPeriod}";
        
        if (hours == eveningHour)
        {
            EveningArrived?.Invoke();
        }

    }

    private void StopTimer()
    {
        _isRunning = false;
        _timeRemaining = 0;
        _sequence?.Kill();
    }
}
