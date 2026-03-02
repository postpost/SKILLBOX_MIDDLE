using Unity.Entities;
using UnityEngine;
using Zenject;

public class SettingsInstaller : MonoInstaller
{
    public bool useScriptableObject = true;
    public Settings _settings;
    public PlayerSettings _playerSettings;
    public override void InstallBindings()
    {
        Debug.Log("SettingsInstaller: InstallBindings");
        if (useScriptableObject)
            Container.Bind<IHealthSettings>().FromInstance(_settings).AsSingle();
        else
            Container.Bind<IHealthSettings>().FromInstance(_playerSettings).AsSingle();

        Container.Bind<IInitializable>().To<SystemInjector>().AsSingle().NonLazy();
    }
}

public class SystemInjector : IInitializable
{
    private DiContainer _container;
    public SystemInjector(DiContainer container)
    {
        _container = container;
    }

    public void Initialize()
    {
        var world = Unity.Entities.World.DefaultGameObjectInjectionWorld;
        if (world != null)
        {
            var system = world.GetExistingSystemManaged<InitSettingsSystem>();
            if (system != null) _container.Inject(system);
        }
    }
}