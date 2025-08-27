using PostProcessing;
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
            
            var runtimeInventory = Instantiate(playerInventory);
            builder.RegisterInstance(runtimeInventory).AsSelf();
        }
    }
}