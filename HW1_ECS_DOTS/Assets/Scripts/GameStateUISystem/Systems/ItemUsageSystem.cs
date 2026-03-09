using Unity.Entities;

public partial struct ItemUsageSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                           .CreateCommandBuffer(state.WorldUnmanaged);

        foreach(var (request, inventory, entity) in SystemAPI.Query<UseItemRequest, DynamicBuffer<InventoryBufferElement>>()
                                                             .WithEntityAccess())
        {
            for(int i = 0 ; i <= inventory.Length; ++i)
            {
                if(request.ItemID == inventory[i].ItemID)
                {
                    inventory.RemoveAt(i);
                    break;
                }
            }
            ecb.RemoveComponent<UseItemRequest>(entity);
        }
    }
}
