using UnityEngine;

namespace ScriptableObjects
{
        [CreateAssetMenu(fileName = "NewDeliveryCar", menuName = "DeliveryCars/DeliveryCarData")]
        public class DeliveryCarData : ScriptableObject
        {
            public float deliveryTime;
        }
    
}