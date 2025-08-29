using Enum;

namespace Strategies
{
    public class DeliveryCarLoadStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not DeliveryCar deliveryCar) return false;
            if (deliveryCar.State is not DeliveryCarState.Empty || !player.Inventory.CanAddCargo) return false;
            deliveryCar.State = DeliveryCarState.Loaded;
            deliveryCar.PutCargo(player.Inventory.harvestObjects);
            deliveryCar.ChangeTile();
            player.Inventory.UseWater();
            player.Inventory.ClearItems();
            return true;
        }
    }
}