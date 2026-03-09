using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class StatsUISystem : SystemBase
{
    private int _lastHealth;
    protected override void OnUpdate()
    {
        foreach (var health in SystemAPI.Query<RefRO<HealthData>>()
                                        .WithAll<PlayerTag>()
                                        .WithChangeFilter<HealthData>())
        {
            int newHealth = (int)health.ValueRO.Value;
            if (StatsUIHandler.Instance != null && StatsUIHandler.Instance.ViewModel != null && _lastHealth !=newHealth)
            {
                StatsUIHandler.Instance.ViewModel.Health = newHealth;
            }
        }
    }
}
