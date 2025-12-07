using Enum;

public class Sheep : Animal
{
    protected override void DoSomethingWhenAdult()
    {
        _animator.SetBool(SpecialPerk, true);
        _currentGrowState = AnimalGrowStates.Special;
        _spriteRenderer.sprite = _animalData._specialStateSprite;
    }
}