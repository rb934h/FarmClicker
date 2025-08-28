using PostProcessing;
using Strategies;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private URPVolume _urpVolume;
        [SerializeField] private PlayerInventoryData playerInventory;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_urpVolume);
            // ------------ Garden ------------------
            builder.Register<IPointerObjectInteractStrategy, GardenSeedStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, GardenPlantedStrategy>(Lifetime.Singleton);
            // ------------ WaterTank ------------------
            builder.Register<IPointerObjectInteractStrategy, WaterTankTakeStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, WaterTankFIllStrategy>(Lifetime.Singleton);
            // ------------ DeliveryCar ------------------
            
            var runtimeInventory = Instantiate(playerInventory);
            builder.RegisterInstance(runtimeInventory).AsSelf();
        }
    }
}