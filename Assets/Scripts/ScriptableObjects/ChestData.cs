using UnityEngine;

namespace ScriptableObjects
{
        [CreateAssetMenu(fileName = "NewChest", menuName = "Chests/ChestData")]
        public class ChestData : ScriptableObject
        {
            public float deliveryTime;
            
            [Header("Sale board")]
            public Sprite saleBoard;
            public float saleBoardFallDistance = 0.1f;
            public float saleBoardDuration = 1f;
            public float saleBoardImpactScale = 0.85f;
            public float saleBoardWobbleAngle = 10f;

            [Header("Coin")] 
            public Sprite coinFront;
            public Sprite coinBack;
            public float coinSpinDuration = 0.6f;
            public bool coinSpinAminationLoop = true;
            public float coinSpinScaleMultiplier = 1.25f;

        }
    
}