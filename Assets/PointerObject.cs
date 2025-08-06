using UnityEngine;

public class PointerObject : MonoBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] private Transform pointForInteraction;
    
    public Collider Collider => collider;
    public Transform PointForInteraction => pointForInteraction;

    public virtual void ChangeState()
    {
      
    }
}
