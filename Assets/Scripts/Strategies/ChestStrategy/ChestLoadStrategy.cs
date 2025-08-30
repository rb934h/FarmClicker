using Enum;
using PointerObjects;

namespace Strategies.ChestStrategy
{
    public class ChestLoadStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Chest chest) return false;
            if (chest.State is not ChestState.Empty || chest.GetCargoCount() == 2 || !player.Inventory.CanAddCargo) return false;

            chest.PutCargo(player.Inventory.harvestObjects);
            player.Inventory.ClearItems();
            chest.State = ChestState.Loaded;

            return true;
        }
    }
}