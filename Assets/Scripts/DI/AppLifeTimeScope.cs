using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class AppLifetimeScope : LifetimeScope
    {
        [SerializeField] private CrossSceneAnimation _crossSceneAnimation;
    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_crossSceneAnimation);
        }
    }
}