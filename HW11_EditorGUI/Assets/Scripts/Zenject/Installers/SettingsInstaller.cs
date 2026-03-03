using UnityEngine;
using Zenject;

public class SettingsInstaller : MonoInstaller
{
    public bool useScriptableObject = true;
    public Settings settings;
    public PlayerSettings playerSettings;

    public override void InstallBindings()
    {
        IHealthSettings settingsToBind = useScriptableObject? (IHealthSettings) settings: playerSettings;
       
        if(settingsToBind == null)
        {
            Debug.Log("Назначьте конфиги в SettingsInstaller");
        }

        Container.Bind<IHealthSettings>().FromInstance(settingsToBind).AsSingle();
        //Container.Bind<IInitializable>().To<SystemInjector>().AsSingle().NonLazy();
        Container.BindInterfacesTo<SystemInjector>().AsSingle().NonLazy();
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