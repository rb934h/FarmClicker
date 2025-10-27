using System;
using Enum;
using PointerObjects;

namespace Strategies.GardenStrategy
{
    public class GardenHarvestStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Garden garden) return false;
            if (garden.State is not GardenState.ReadyToHarvest || !player.inventory.canAddItem) return false;
            player.inventory.AddHarvestObject(garden.GetHarvestObject());
            player.animator.PlayAnimation(PlayerAnimationState.PlayerWeeding);
            garden.Remove();
            OnComplete?.Invoke();
            return true;
        }
    }
}
