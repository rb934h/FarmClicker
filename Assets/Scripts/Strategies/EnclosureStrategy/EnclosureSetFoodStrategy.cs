using System;
using Enum;
using PointerObjects;

namespace Strategies.EnclosureStrategy
{
    public class EnclosureSetFoodStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player.Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Enclosure enclosure) return false;
            if (enclosure.HasFood || !player.inventory.handsNotEmpty) return false;

            foreach (var inventoryHarvestObject in player.inventory.harvestObjects)
            {
                if (inventoryHarvestObject as PlantData)
                {
                    enclosure.SetFoodToBowl();
                    player.inventory.ClearItem(inventoryHarvestObject);
                    OnComplete?.Invoke();
                    return true;
                }
            }
         
            return false;
        }
    }
}