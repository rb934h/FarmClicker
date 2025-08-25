using DefaultNamespace;
using DG.Tweening;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 0.1f;
    [SerializeField] private URPVolume _urpVolume;
    
    private CanvasGroup _canvasGroup;
    private bool _isOpen = false;
    
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isOpen)
                Hide();
            else
                Show();
        }
    }

    private void Show()
    {
        _isOpen = true;
        _canvasGroup.DOFade(1, _fadeDuration);
        ToggleEffects();
    }

    private void Hide()
    {
        _isOpen = false;
        _canvasGroup.DOFade(0, _fadeDuration);
        ToggleEffects();
    }

    private void ToggleEffects()
    {
        _urpVolume.ChangeChromaticAberrationValue(_fadeDuration);
        _urpVolume.ChangeDepthOfFieldValue(_fadeDuration);
    }
}