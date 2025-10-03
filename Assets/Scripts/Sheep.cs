

namespace DefaultNamespace
{
    public class Sheep : Animal
    {
        protected override void DoSomethingWhenAdult()
        {
            base.DoSomethingWhenAdult();
            _spriteRenderer.sprite = _animalData._withWoolSprite;
        }
    }
}