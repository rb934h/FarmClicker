using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class VolumeSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Start()
        {
            _musicSlider.onValueChanged.AddListener(SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(SetSFXVolume);

            _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }

        private void SetMusicVolume(float value)
        {
            _mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat("MusicVolume", value);
        }

        private void SetSFXVolume(float value)
        {
            _mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }
}