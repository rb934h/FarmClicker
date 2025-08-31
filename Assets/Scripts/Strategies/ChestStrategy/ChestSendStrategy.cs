using System.Linq;
using Enum;
using PointerObjects;

namespace Strategies.ChestStrategy
{
    public class ChestSendStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Chest chest) return false;
            if (chest.State is not ChestState.Loaded || chest.WaitForSale) return false;
            
            if (player.Inventory.handsNotEmpty && chest.GetCargoCount() != 2)
            {
                foreach (var inventoryHarvestObject in player
                             .Inventory
                             .harvestObjects
                             .ToList()
                             .Where(inventoryHarvestObject => chest.PutCargo(inventoryHarvestObject)))
                {
                    player.Inventory.ClearItem(inventoryHarvestObject);
                }

                return true;
            }
              
            chest.ChangeTile();
            chest.Send();
            return true;
        }
    }
}