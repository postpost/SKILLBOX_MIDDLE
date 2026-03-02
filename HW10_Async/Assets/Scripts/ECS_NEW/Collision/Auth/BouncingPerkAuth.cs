using Unity.Entities;
using UnityEngine;

public class BouncingPerkAuth : MonoBehaviour
{
    public class BouncingPerkBaker : Baker<BouncingPerkAuth>
    {
        public override void Bake(BouncingPerkAuth authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<BouncingPerkPickupTag>(entity);
        }
    }

}
