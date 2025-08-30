using UnityEngine;

namespace ScriptableObjects
{
        [CreateAssetMenu(fileName = "NewChest", menuName = "Chests/ChestData")]
        public class ChestData : ScriptableObject
        {
            public float deliveryTime;
            
            [Header("Sale board")]
            public Sprite saleBoard;
            public float fallDistance = 0.1f;
            public float duration = 1f;
            public float impactScale = 0.85f;
            public float wobbleAngle = 10f;
        }
    
}