using Enum;
using ScriptableObjects;

namespace Animals
{
    public class Sheep : Animal
    {
        protected override void DoSomethingWhenAdult()
        {
            _animator.SetBool(SpecialPerk, true);
            _currentGrowState = AnimalGrowStates.Special;
            _spriteRenderer.sprite = _animalData._specialStateSprite;
        }

        public override CollectableItemData GetSpecialItem()
        {
            _spriteRenderer.sprite = _animalData._adultSprite;
            _animator.SetBool(SpecialPerk, false);
            
            return base.GetSpecialItem();
        }
    }
}