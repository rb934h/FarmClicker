using System;
using Enum;
using PointerObjects;

namespace Strategies.EnclosureStrategy
{
    public class EnclosureGetSpecialItemStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Enclosure enclosure) return false;
            if (!player.inventory.canAddItem) return false;
            foreach (var enclosureAnimal in enclosure.Animals)
            {
                if (enclosureAnimal._currentGrowState == AnimalGrowStates.Special)
                {
                    player.inventory.AddHarvestObject(enclosureAnimal.GetSpecialItem());
                    OnComplete?.Invoke();
                    return true;
                }
            }
            return false;
        }
    }
}