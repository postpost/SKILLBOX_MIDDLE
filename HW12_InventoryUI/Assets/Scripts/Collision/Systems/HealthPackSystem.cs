using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct HealthPackSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //1. create entity buffer to destroy pack 
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        //collecl all who is can be healed
        var playerQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, DamageableTag>().Build();
        //WorldUpdateAllocator -- same as Allocator.Temp but with garantee
        var playerTransforms = playerQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);
        var playerEntities = playerQuery.ToEntityArray(state.WorldUpdateAllocator);

        //healthpack
        //WithEntityAccess gives access to the entity to remove it
        foreach (var (packTransform, packData, packEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<HealthPackData>>().WithEntityAccess())
        {
            var packPos = packTransform.ValueRO.Position;
            var packRadius = packData.ValueRO.Radius;
            var sqrtRadius = packRadius * packRadius;

            for(int i = 0; i < playerTransforms.Length; ++i)
            {
                float3 playerPos = playerTransforms[i].Position;

                if(math.distancesq(playerPos, packPos) <= sqrtRadius)
                {
                    //heal
                    var health = SystemAPI.GetComponent<HealthData>(playerEntities[i]);
                    health.Value = math.min(health.Value +packData.ValueRO.HealthAmount, health.MaxHealth);
                    SystemAPI.SetComponent(playerEntities[i], health);

                    ecb.DestroyEntity(packEntity); //here is just put a tick
                    Debug.Log($"аптечка подобрана, current health: {health.Value}");
                    break;
                }
            }
        }
            ecb.Playback(state.EntityManager); //run all commands at the end (not in the cycle)
            ecb.Dispose(); // and now definitely delete all
    }


}
