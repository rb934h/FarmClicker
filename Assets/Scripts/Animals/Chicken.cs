using DG.Tweening;
using Enum;
using ScriptableObjects;
using UnityEngine;

namespace Animals
{
    public class Chicken : Animal
    {
        [SerializeField] private GameObject _specialItemPrefab;

        private readonly Vector2 _specialPerkDelayRange = new Vector2(5f, 10f);
        private GameObject _specialItemObject;

        protected override void DoSomethingWhenAdult()
        {
            var specialPerkDelay = Random.Range(_specialPerkDelayRange.x, _specialPerkDelayRange.y);
            _currentGrowState = AnimalGrowStates.Special;

            DOVirtual.DelayedCall(specialPerkDelay,
                () =>
                {
                    _specialItemObject = Instantiate(_specialItemPrefab, transform.position, Quaternion.identity,
                        transform.parent);
                });
        }

        public override CollectableItemData GetSpecialItem()
        {
            Destroy(_specialItemObject);
            return base.GetSpecialItem();;
        }
    }
}