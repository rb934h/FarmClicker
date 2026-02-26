using DefaultNamespace;
using PostProcessing;
using Strategies;
using Strategies.ChestStrategy;
using Strategies.EnclosureStrategy;
using Strategies.GardenStrategy;
using Strategies.WaterTankStrategy;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace DI.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [FormerlySerializedAs("urpVolume")] [SerializeField] private URPVolume _urpVolume;
        [FormerlySerializedAs("player")] [SerializeField] private Player.Player _player;
        [FormerlySerializedAs("inputSystem")] [SerializeField] private InputSystem _inputSystem;
        [FormerlySerializedAs("levelTimer")] [SerializeField] private Timer _levelTimer;
        [FormerlySerializedAs("dialogueManager")] [SerializeField] private DialogueManager _dialogueManager;
        [FormerlySerializedAs("audioPlayer")] [SerializeField] private AudioPlayer _audioPlayer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_urpVolume);
            builder.RegisterComponent(_player);
            builder.RegisterComponent(_levelTimer);
            builder.RegisterComponent(_dialogueManager);
            builder.RegisterComponent(_audioPlayer);
            
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
            builder.RegisterComponent(_inputSystem);
        }
    }
}
