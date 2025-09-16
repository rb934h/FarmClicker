using PointerObjects;

namespace Strategies.EnclosureStrategy
{
    public class EnclosureSetWaterStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Enclosure enclosure) return false;
            if (enclosure.HasWater || !player.inventory.hasWater) return false;
            enclosure.SetWaterToBowl();
            player.inventory.UseWater();
            return true;
        }
    }
}