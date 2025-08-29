using Enum;
using PointerObjects;

namespace Strategies.DeliveryCarStrategy

{
    public class DeliveryCarWithMoneyStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not PointerObjects.DeliveryCar deliveryCar) return false;
            if (deliveryCar.State is not DeliveryCarState.WithMoney) return false;
            player.Inventory.AddCoins(deliveryCar.GetCargoPrice());
            deliveryCar.ClearCargo();
            deliveryCar.State = DeliveryCarState.Empty;
            return true;
        }
    }
}