using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    private Material _material;
    
    private bool _isTransitioning = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _material = GetComponentInChildren<Image>().material;
    }
    
    public void LoadLevel(int levelIndex)
    {
        if (_isTransitioning) return;
        _isTransitioning = true;
        
        _material.SetFloat("_ScreenAspect", (float)Screen.width / Screen.height);
        _material.DOFloat(0, "_Radius", _duration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(levelIndex);
                _material.DOFloat(1, "_Radius", _duration)
                    .SetEase(Ease.InOutSine);
            });
    }
}
