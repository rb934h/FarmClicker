using System;
using System.Linq;
using Enum;
using PointerObjects;

namespace Strategies.ChestStrategy
{
    public class ChestLoadStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Chest chest) return false;
            if (chest.State is not ChestState.Empty || chest.GetCargoCount() >= 2 || !player.inventory.handsNotEmpty) return false;
            
            foreach (var inventoryHarvestObject in player
                         .inventory
                         .harvestObjects
                         .ToList()
                         .Where(inventoryHarvestObject => chest.PutCargo(inventoryHarvestObject)))
            {
                player.inventory.ClearItem(inventoryHarvestObject);
            }
            
            chest.State = ChestState.Loaded;
            OnComplete?.Invoke();
            return true;
        }
    }
}