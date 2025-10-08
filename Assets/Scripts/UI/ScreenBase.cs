using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScreenBase : MonoBehaviour
{
    [SerializeField] private float _duration = .5f;

    public static List<ScreenBase> Screens = new List<ScreenBase>();
    private CanvasGroup _canvasGroup;

    protected void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Screens.Add(GetComponent<ScreenBase>());
    }

    public virtual void ShowScreen()
    {
        _canvasGroup.DOFade(1, _duration);
    }
    
    public virtual void HideScreen()
    {
        _canvasGroup.DOFade(0, _duration);
    }
}
