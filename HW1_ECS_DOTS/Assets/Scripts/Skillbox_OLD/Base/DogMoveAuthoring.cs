using Unity.Entities;
using UnityEngine;

public class DogMoveAuthoring : MonoBehaviour
{
    public float speed;

    //insted of ConvertToEntity
    public class DogMoveBaker : Baker<DogMoveAuthoring>
    {
        public override void Bake(DogMoveAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new DogMoveComponent { Speed = authoring.speed });
        }
    }
}