using Enum;
using PointerObjects;

namespace Strategies.ChestStrategy
{
    public class ChestLoadStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Chest chest) return false;
            if (chest.State is not ChestState.Empty || !player.Inventory.CanAddCargo) return false;
            chest.State = ChestState.Loaded;
            chest.PutCargo(player.Inventory.harvestObjects);
            chest.ChangeTile();
            player.Inventory.UseWater();
            player.Inventory.ClearItems();
            return true;
        }
    }
}