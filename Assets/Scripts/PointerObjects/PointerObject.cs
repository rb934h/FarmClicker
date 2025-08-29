using DG.Tweening;
using TMPro;
using UnityEngine;

public class PointerObject : MonoBehaviour
{
    [Header("Pointer object settings")]
    [SerializeField] private Collider2D pointerObjectCollider;
    [SerializeField] private Transform pointForInteraction;
    [SerializeField] protected TMP_Text _stateInfoText;
    
    public Collider2D Collider => pointerObjectCollider;
    public Transform PointForInteraction => pointForInteraction;
    

    protected void ShowStateInfo(string text)
    {
        _stateInfoText.text = text;
        _stateInfoText.DOFade(1, 0.3f);
    }
    
    protected void HideStateInfo()
    {
        _stateInfoText.DOFade(0, 0.3f);
    }
}
