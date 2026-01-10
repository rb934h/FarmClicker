using System;
using PointerObjects;

namespace Strategies
{
    public class PigResetHandsStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player.Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Pig pig) return false;
            player.inventory.ClearAllItems();
            OnComplete?.Invoke();
            return true;
        }
    }
}