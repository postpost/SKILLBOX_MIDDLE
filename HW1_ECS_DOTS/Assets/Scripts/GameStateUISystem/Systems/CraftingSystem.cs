using UnityEngine;
using Unity.Entities;

public partial struct CraftingSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                           .CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (request, inventory, recipe, selectedItems, entity) in SystemAPI.Query<CraftRequest,
                                                DynamicBuffer<InventoryBufferElement>,
                                                DynamicBuffer<CraftIngredienstBuffer>,
                                                DynamicBuffer<SelectedItemElement>>()                                                
                                                .WithEntityAccess())
        {
            //сравниваем запрос и имеющийся инвентарь
            bool canCraft = true;
            foreach(var ingredient in recipe)
            {
                int selectedCount = 0;
                foreach(var s in selectedItems)
                {
                   if(ingredient.ItemID == s.ItemID) ++selectedCount;
                }

                if(selectedCount < ingredient.Count) 
                {
                    canCraft = false;
                    break;
                }
            }
            if (canCraft)
            {
                foreach (var ingredient in recipe)
                {
                    int removed = 0;
                    for (int i = inventory.Length - 1; i >= 0 && removed < ingredient.Count; --i)
                    {
                        if (inventory[i].ItemID != ingredient.ItemID) continue;
                        inventory.RemoveAt(i);
                        ++removed;
                    }
                }
                inventory.Add(new InventoryBufferElement
                {
                    ItemID = request.ResultID
                });
                Debug.Log("Crafting is successfull");
            }
            else
            {
                Debug.Log("Crafting is NOT successfull");
            }

            ecb.RemoveComponent<CraftRequest>(entity);
            ecb.RemoveComponent<CraftIngredienstBuffer>(entity);
            ecb.RemoveComponent<SelectedItemElement>(entity);
        }
    }
}
