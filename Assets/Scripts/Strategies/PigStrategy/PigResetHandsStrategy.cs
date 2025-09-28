using PointerObjects;

namespace Strategies
{
    public class PigResetHandsStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Pig pig) return false;
            player.inventory.ClearAllItems();
            return true;
        }
    }
}