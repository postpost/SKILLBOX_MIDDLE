using Unity.Entities;
using UnityEngine;
using Zenject;

public partial class InitSettingsSystem : SystemBase
{
    private IHealthSettings _settings;

    [Inject] //команда для Zenject: вколи данные сюда из настроек биндинга (у себя в контейнере)
    public void Construct(IHealthSettings settings)
    {
        _settings = settings;
    }

    protected override void OnUpdate()
    {
        if(_settings == null) return;
        foreach(var (health, entity) in SystemAPI.Query<RefRW<HealthData>>().WithEntityAccess())
        {
            if(health.ValueRO.MaxHealth == 0)
            {
                health.ValueRW.MaxHealth = _settings.MaxHealth;
                health.ValueRW.Value = _settings.MaxHealth;
            }
        }
    }

}
