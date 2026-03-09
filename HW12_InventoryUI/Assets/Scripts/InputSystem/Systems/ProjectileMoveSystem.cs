using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ProjectileMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        //способ перебора всех пуль через Query
        foreach (var (transform, speed, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<ProjectileMoveSpeed>>().WithEntityAccess())
        {
            // math.forward(rotation) возвращает вектор "вперед" для текущего поворота пули
            float3 forward = math.forward(transform.ValueRO.Rotation);
            float distance = speed.ValueRO.Value * deltaTime;
            bool hitWall = false;

            if (Physics.Raycast(transform.ValueRO.Position, (Vector3)forward, out RaycastHit hit, distance + 0.1f))
            {
                //if has perk
                if (SystemAPI.HasComponent<BouncingPerkTag>(entity))
                {
                    float3 normal = hit.normal;
                    float3 reflectDir = math.reflect(forward, normal);

                    transform.ValueRW.Rotation = quaternion.LookRotationSafe(reflectDir, math.up());
                    transform.ValueRW.Position = (float3)hit.point + reflectDir * 0.05f;
                    hitWall = true;
                }
                else
                {
                    ecb.DestroyEntity(entity);
                    hitWall = true;
                }
                continue;
            }

            if (!hitWall)
            {
                // Двигаем позицию: направление * скорость * время
                transform.ValueRW.Position += forward * distance;
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
