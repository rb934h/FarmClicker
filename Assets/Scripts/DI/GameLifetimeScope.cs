using PostProcessing;
using Strategies;
using Strategies.ChestStrategy;
using Strategies.EnclosureStrategy;
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
        [SerializeField] private Player player;
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private Timer levelTimer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(urpVolume);
            builder.RegisterComponent(player);
            builder.RegisterComponent(levelTimer);
            
            // ------------ Pig ------------------
            builder.Register<IPointerObjectInteractStrategy, PigResetHandsStrategy>(Lifetime.Singleton);
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
            // ------------ Enclosure ------------------
            builder.Register<IPointerObjectInteractStrategy, EnclosureSetFoodStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, EnclosureSetWaterStrategy>(Lifetime.Singleton);
            builder.Register<IPointerObjectInteractStrategy, EnclosureGetSpecialItemStrategy>(Lifetime.Singleton);
            
            // ------------ Other ------------------
            builder.RegisterComponent(inputSystem);
        }
    }
}