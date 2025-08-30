using PostProcessing;
using Strategies;
using Strategies.ChestStrategy;
using Strategies.GardenStrategy;
using Strategies.WaterTankStrategy;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private URPVolume urpVolume;
        [SerializeField] private PlayerInventoryData playerInventory;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(urpVolume);
            
            // ------------ Garden ------------------
            builder.Register<IPointerObjectInteractStrategy, GardenSeedStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, GardenPlantedStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, GardenHarvestStrategy>(Lifetime.Singleton);
            // ------------ WaterTank ------------------
            builder.Register<IPointerObjectInteractStrategy, WaterTankFIllStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, WaterTankTakeStrategy>(Lifetime.Singleton);
            // ------------ Chest ------------------
            builder.Register<IPointerObjectInteractStrategy, ChestLoadStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, ChestSendStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, ChestWithMoneyStrategy>(Lifetime.Singleton);
            
            var runtimeInventory = Instantiate(playerInventory);
            builder.RegisterInstance(runtimeInventory).AsSelf();
        }
    }
}