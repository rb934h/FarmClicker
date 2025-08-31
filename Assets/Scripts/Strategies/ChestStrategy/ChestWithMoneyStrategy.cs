using Enum;
using PointerObjects;

namespace Strategies.ChestStrategy

{
    public class ChestWithMoneyStrategy : IPointerObjectInteractStrategy
    {
        public bool Interact(Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Chest chest) return false;
            if (chest.State is not ChestState.WithMoney) return false;
            chest.ClearCargo();
            chest.State = ChestState.Empty;
            return true;
        }
    }
}