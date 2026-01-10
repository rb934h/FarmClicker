using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _ambientAudioSource;
    [SerializeField] private AudioSource _backgroundAudioSource;
    [SerializeField] private AudioClip[] _ambientAudioClips;
    [SerializeField] private AudioClip[] _backgroundAudioClips;
    [Space]
    [SerializeField] private float _fadeDuration = 1.5f;

    private Coroutine _fadeRoutine;
        
    public void PlayNightSounds() => PlayAmbient(1);
    public void PlayDaySounds() => PlayAmbient(0);
    
    private void PlayAmbient(int index)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeToNewClips(index));
    }

    private IEnumerator FadeToNewClips(int index)
    {
        var hasPlayedBefore =
            _ambientAudioSource.clip != null ||
            _backgroundAudioSource.clip != null;

        var startAmbientVol = _ambientAudioSource.volume;
        var startBackgroundVol = _backgroundAudioSource.volume;
        
        if (hasPlayedBefore && (startAmbientVol > 0f || startBackgroundVol > 0f))
        {
            for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
            {
                var normalized = t / _fadeDuration;
                _ambientAudioSource.volume = Mathf.Lerp(startAmbientVol, 0, normalized);
                _backgroundAudioSource.volume = Mathf.Lerp(startBackgroundVol, 0, normalized);
                yield return null;
            }
        }
        
        _ambientAudioSource.clip = _ambientAudioClips[index];
        _backgroundAudioSource.clip = _backgroundAudioClips[index];

        _ambientAudioSource.Play();
        _backgroundAudioSource.Play();

        for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
        {
            var normalized = t / _fadeDuration;
            _ambientAudioSource.volume = Mathf.Lerp(_ambientAudioSource.volume, startAmbientVol, normalized);
            _backgroundAudioSource.volume = Mathf.Lerp(_backgroundAudioSource.volume, startBackgroundVol, normalized);
            yield return null;
        }

        _ambientAudioSource.volume = 1f;
        _backgroundAudioSource.volume = 1f;
    }
}