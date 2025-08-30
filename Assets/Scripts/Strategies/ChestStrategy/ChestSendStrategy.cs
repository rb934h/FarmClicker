using Enum;
using PointerObjects;

namespace Strategies.ChestStrategy
{
    public class ChestSendStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Chest chest) return false;
            if (chest.State is not ChestState.Loaded) return false;
            
            if (player.Inventory.CanAddCargo && chest.GetCargoCount() != 2)
            {
                chest.PutCargo(player.Inventory.harvestObjects);
                player.Inventory.ClearItems();
                return true;
            }
              
            chest.ChangeTile();
            chest.Send();
            return true;
        }
    }
}