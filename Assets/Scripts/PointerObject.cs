using DG.Tweening;
using TMPro;
using UnityEngine;

public class PointerObject : MonoBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] private Transform pointForInteraction;
    [SerializeField] protected TMP_Text _stateInfoText;
    
    public Collider Collider => collider;
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
