using Unity.Entities;
using UnityEngine;

public class PickupItemAuth : MonoBehaviour
{
    [SerializeField] private ItemStaticData staticData; //связь с конкрентым предметом инвентаря
    public class PickupBaker : Baker<PickupItemAuth>
    {
        public override void Bake(PickupItemAuth authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent<CollectableItemTag>(entity);
            AddComponent(entity, new ItemID
            {
                ID = authoring.staticData.ItemID
            });
        }
    }
}

public struct CollectableItemTag : IComponentData { }
public struct ItemID:IComponentData 
{ 
    public int ID;
}

public struct UseItemRequest: IComponentData 
{
    public int ItemID;
}