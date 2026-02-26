using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace PointerObjects
{
    public abstract class PointerObject : MonoBehaviour
    {
        [Header("Pointer object settings")]
        [FormerlySerializedAs("pointerObjectCollider")] [SerializeField] protected Collider2D _pointerObjectCollider;
        [FormerlySerializedAs("pointForInteraction")] [SerializeField] private Transform _pointForInteraction;
        [FormerlySerializedAs("selectedSpriteRenderer")] [SerializeField] protected SpriteRenderer _selectedSpriteRenderer;
        
        protected float _workTime;
    
        public Collider2D Collider => _pointerObjectCollider;
        public Transform PointForInteraction => _pointForInteraction;
        public SpriteRenderer SelectedSpriteRenderer => _selectedSpriteRenderer;
        public float WorkTime => _workTime;
    }
}
