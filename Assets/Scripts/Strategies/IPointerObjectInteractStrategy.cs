using System;
using PointerObjects;

namespace Strategies
{
    public interface IPointerObjectInteractStrategy
    {
        public event Action OnComplete;
        public bool Interact(Player.Player player, PointerObject pointerObject);
    }
}