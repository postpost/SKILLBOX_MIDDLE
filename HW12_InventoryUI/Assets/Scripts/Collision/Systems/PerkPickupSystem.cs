using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;


public partial struct PerkPickupSystem : ISystem
{
   //будет проверять, есть ли тэг на bouncetag + давайть игроку тег подбора перка
   public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        //find player with tag damageable and his pos
        foreach(var (playerTransform, playerEntity) in 
            SystemAPI.Query<RefRO<LocalTransform>>().WithAll<DamageableTag>().WithEntityAccess())
        {
            foreach (var (parkTransform, perkEntity) in
                SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BouncingPerkPickupTag>().WithEntityAccess())
            {
                //pickup
                if(math.distancesq(playerTransform.ValueRO.Position, parkTransform.ValueRO.Position) <= 1f)
                {
                    ecb.AddComponent<PlayerHasBouncingPerk>(playerEntity);
                    ecb.DestroyEntity(perkEntity);
                    Debug.Log("perk is picked up");
                    break;
                }
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
