using UnityEngine;
using UnityEngine.EventSystems;

namespace PointerObjects
{
    public abstract class PointerObject : MonoBehaviour
    {
        [Header("Pointer object settings")]
        [SerializeField] protected Collider2D pointerObjectCollider;
        [SerializeField] private Transform pointForInteraction;
        [SerializeField] protected SpriteRenderer selectedSpriteRenderer;
        
        protected  float _workTime;
    
        public Collider2D Collider => pointerObjectCollider;
        public Transform PointForInteraction => pointForInteraction;
        public SpriteRenderer SelectedSpriteRenderer => selectedSpriteRenderer;
        public float WorkTime => _workTime;
    }
}