using Enum;

namespace DefaultNamespace
{
    public class Sheep : Animal
    {
        protected override void DoSomethingWhenAdult()
        {
            _spriteRenderer.sprite = _animalData._withWoolSprite;
            _animator.Play("WithWoolSheepRun");
        }
    }
}