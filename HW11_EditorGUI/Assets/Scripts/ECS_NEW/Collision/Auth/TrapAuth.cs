using UnityEngine;
using Unity.Entities;

public class TrapAuth : MonoBehaviour
{
    public float radius = 2f;
    public float damage = 10;

    public class TrapAuthBaker : Baker<TrapAuth>
    {
        public override void Bake(TrapAuth authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TrapData
            {
                Radius = authoring.radius,
                Damage = authoring.damage,
            });
        }
    }
}
