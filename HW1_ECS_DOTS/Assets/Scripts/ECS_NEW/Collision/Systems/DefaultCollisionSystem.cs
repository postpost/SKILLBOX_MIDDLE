using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public partial struct DefaultCollisionSystem : ISystem
{
   private const float TAU = math.PI * 2;
   
    public void OnUpdate(ref SystemState state)
    {
        DrawTrapGizmos(ref state);
        //all damageable
        var damageableQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, HealthData, DamageableTag>().Build();
        // collect their transforms to check distance
        var damageableTransformArray = damageableQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);
        var damageableEntities = damageableQuery.ToEntityArray(state.WorldUpdateAllocator);

        //all traps
        foreach (var (agent, trapData) in SystemAPI.Query<AIAgent, RefRO<TrapData>>())
        {
            float3 trapPos = agent.Manager.transform.position;
            float trapRadius = trapData.ValueRO.Radius;
            float sqrtRadius = trapRadius * trapRadius;

            for(int i =0;  i < damageableTransformArray.Length; i++)
            {
                float3 damageablePos = damageableTransformArray[i].Position;

                if (math.distancesq(damageablePos, trapPos) <= sqrtRadius)
                {
                    Entity damagedEntity = damageableEntities[i];
                    var health = SystemAPI.GetComponent<HealthData>(damagedEntity);

                    health.Value -= (trapData.ValueRO.Damage * SystemAPI.Time.DeltaTime);
                    health.Value = math.clamp(health.Value, 0, health.MaxHealth);
                    SystemAPI.SetComponent(damagedEntity, health);
                    
                    if ((SystemAPI.HasComponent<DeathTag>(damagedEntity) || 
                        !state.EntityManager.IsComponentEnabled<DeathTag>(damagedEntity)) && 
                        health.Value <= 0)
                    {
                        SystemAPI.SetComponentEnabled<DeathTag>(damagedEntity, true);
                        break; //not sure
                    }

                    if(SystemAPI.HasComponent<GetHit>(damagedEntity))
                        SystemAPI.SetComponentEnabled<GetHit>(damagedEntity, true); //в чем разница при активации с EntityManager
                }
            }
        }
    }

    
    private void DrawTrapGizmos(ref SystemState state)
    {
        float time = (float)SystemAPI.Time.ElapsedTime;
        float pulse = math.sin(time * 3f) * 0.2f; //time * 3f = speed, 0.2 - expansion/amplitude

        foreach (var (agent, trapData) in SystemAPI.Query<AIAgent, RefRO<TrapData>>())
        {
            float3 trapPos = agent.Manager.transform.position;
            float radius = trapData.ValueRO.Radius + pulse;

            int segments = 16;
            for(int a= 0; a < segments; ++a)
            {
                float angle = a * TAU / segments;
                float nextAngle = (a+1) * TAU / segments;
                float3 start = trapPos + new float3(math.cos(angle) * radius, 0, math.sin(angle) * radius);
                float3 end = trapPos + new float3(math.cos(nextAngle) * radius, 0, math.sin(nextAngle) * radius);

                Color debugColor = Color.Lerp(Color.red, Color.yellow, math.abs(pulse) * 5f);
                Debug.DrawLine(start, end, debugColor);
                
            }
        }
    }
}
