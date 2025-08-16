using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PointerObject : MonoBehaviour
{
    [SerializeField] private Collider pointerObjectCollider;
    [SerializeField] private Transform pointForInteraction;
    [SerializeField] protected TMP_Text _stateInfoText;
    
    public bool IsAvailable = true;
    
    public Collider Collider => pointerObjectCollider;
    public Transform PointForInteraction => pointForInteraction;
    
    public virtual void ChangeState()
    {
      
    }

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
