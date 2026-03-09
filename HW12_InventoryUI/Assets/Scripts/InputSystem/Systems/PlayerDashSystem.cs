using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PlayerDashSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, factor, entity) in 
                SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerDashFactor>>()
                .WithAll<PlayerDashTag>()
                .WithEntityAccess())
        {
            float3 forward = math.forward(transform.ValueRO.Rotation);
            transform.ValueRW.Position += forward * factor.ValueRO.Value;

            //SWITCH OFF = 1 frame dash
            SystemAPI.SetComponentEnabled<PlayerDashTag>(entity, false);
        }
    }
}
