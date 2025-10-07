using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private List<Image> _images = new List<Image>();
    [SerializeField] private TMP_Text _timeText;
    
    private float _timeRemaining;
    private bool _isRunning;
    private Sequence _sequence;
    
    public event Action OnTimerComplete;
    
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

        float segmentDuration = _timeRemaining / (_images.Count - 1);
        _sequence = DOTween.Sequence();
        
        for (int i = 0; i < _images.Count - 1; i++)
        {
            float startTime = i * segmentDuration;

            _sequence.Insert(startTime, _images[i].DOFade(0f, segmentDuration).SetEase(Ease.Linear));
            _sequence.Insert(startTime, _images[i + 1].DOFade(1f, segmentDuration).SetEase(Ease.Linear));
        }

        
        float startMinutes = 10f * 60f;
        float endMinutes = 20f * 60f;
        float currentMinutes = startMinutes;

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
        for (int i = 0; i < _images.Count; i++)
        {
            var color = _images[i].color;
            color.a = (i == 0) ? 1f : 0f;
            _images[i].color = color;
        }
    }

    private void UpdateTimeText(float totalMinutes)
    {
        int hours = Mathf.FloorToInt(totalMinutes / 60f);
        int minutes = Mathf.FloorToInt(totalMinutes % 60f);

        string period = hours >= 12 ? "PM" : "AM";
        
        int displayHours = hours % 12;
        if (displayHours == 0) displayHours = 12;

        _timeText.text = $"{displayHours:00} {period}";
    }

    private void StopTimer()
    {
        _isRunning = false;
        _timeRemaining = 0;
        _sequence?.Kill();
    }
}
