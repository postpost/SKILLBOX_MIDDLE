using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PickupSystem : ISystem
{
    public void OnUpdate(ref SystemState state) 
    { 
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                            .CreateCommandBuffer(state.WorldUnmanaged);

        var playerEntity= SystemAPI.GetSingletonEntity<CharacterDataComponent>();
        var inventory = SystemAPI.GetBuffer<InventoryBufferElement>(playerEntity);
        var playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        
        var characterData = SystemAPI.GetComponentRW<CharacterDataComponent>(playerEntity);
        
        foreach (var (transform, item, itemEntity) in SystemAPI.Query<
                                            RefRW<LocalTransform>,
                                            ItemID>()
                                            .WithEntityAccess())
       {
            if (math.distancesq(playerPos, transform.ValueRO.Position) <= 1f)
            {
                inventory.Add(new InventoryBufferElement
                {
                    ItemID = item.ID
                });
                characterData.ValueRW.Score += 3;
                ecb.DestroyEntity(itemEntity);
               // Debug.Log($"item {item.ID} was pickedup");
            }
       }
    }
}
