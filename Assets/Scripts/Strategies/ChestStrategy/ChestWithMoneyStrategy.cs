using System;
using Enum;
using PointerObjects;

namespace Strategies.ChestStrategy

{
    public class ChestWithMoneyStrategy : IPointerObjectInteractStrategy
    {
        public event Action OnComplete;

        public bool Interact(Player.Player player, PointerObject pointerObject)
        {
            if (pointerObject is not Chest chest) return false;
            if (chest._state is not ChestState.WithMoney) return false;
            chest.ClearCargo();
            chest._state = ChestState.Empty;
            OnComplete?.Invoke();
            return true;
        }
    }
}
