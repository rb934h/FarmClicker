using Enum;

namespace Strategies
{
    public class DeliveryCarSendStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not DeliveryCar deliveryCar) return false;
            if (deliveryCar.State is not DeliveryCarState.Loaded) return false;
            deliveryCar.Send();
            return true;
        }
    }
}