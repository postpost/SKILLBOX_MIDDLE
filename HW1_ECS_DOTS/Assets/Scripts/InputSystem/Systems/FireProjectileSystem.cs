using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using static CharacterDataAuth;

//system Instantiates in position projectile
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct FireProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //создает список отложенных задач, память выделяется мгновенно и очищается в конце кадра
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (projectilePrefab, entity) in
            SystemAPI.Query<ProjectilePrefab>().WithEntityAccess().WithAll<FireProjectileTag>())
        {
            //получает мировые координаты точки выстрела
            var shootPointTransform = SystemAPI.GetComponent<LocalToWorld>(projectilePrefab.ShootPointEntity);
            
            //инстанциирует сам префаб пули
            var newProjectile = ecb.Instantiate(projectilePrefab.Value);
            
            //применяет трансформ для пули при ее создании
            var projectileTransform = LocalTransform.FromPositionRotationScale
                                        (shootPointTransform.Position, shootPointTransform.Rotation, 0.5f);

            ecb.SetComponent(newProjectile, projectileTransform);
            
            //bouncing perk
            if(SystemAPI.HasComponent<PlayerHasBouncingPerk>(entity))
            {
                //Debug.Log("ecb.AddComponent<BouncingPerkTag>(newProjectile)");
                ecb.AddComponent<BouncingPerkTag>(newProjectile);
            }

            if (SystemAPI.HasComponent<CharacterDataComponent>(entity))
            {
                var characterData = SystemAPI.GetComponentRW<CharacterDataComponent>(entity);
                characterData.ValueRW.Score += 10;
            }

            //выключает стрельбу в конце кадра
            ecb.SetComponentEnabled<FireProjectileTag>(entity, false);
        }

        //выполняет все накопленные команды разом
        ecb.Playback(state.EntityManager);
        //очищает память, выделенную под список задач (из буфера)
        ecb.Dispose();
    }
}
