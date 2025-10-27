using System;
using Enum;
using PointerObjects;

namespace Strategies.GardenStrategy
{
    public class GardenPlantedStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Garden garden) return false;
            if (garden.State is not GardenState.Planted || !player.inventory.hasWater) return false;
            player.inventory.UseWater();
            player.animator.PlayAnimation(PlayerAnimationState.PlayerWatering);
            garden.StartCoroutine(garden.WaterAndGrow());
            OnComplete?.Invoke();
            return true;
        }
    }
}