using PostProcessing;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] protected URPVolume _urpVolume;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_urpVolume);
        }
    }
}