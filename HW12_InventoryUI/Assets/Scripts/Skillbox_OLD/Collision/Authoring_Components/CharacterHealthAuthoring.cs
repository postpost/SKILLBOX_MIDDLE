using System.Diagnostics;
using Unity.Entities;
using UnityEngine;

public class CharacterHealthAuthoring : MonoBehaviour
{
    public int health = 100;
    public Entity selfRef;

    public class CharacterHealthBaker : Baker<CharacterHealthAuthoring>
    {
        public override void Bake(CharacterHealthAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic); //физика всегда требует Transform
            authoring.selfRef = entity;
            AddComponentObject(entity, authoring);
            AddComponent(entity, new HealthData
            {
                Value = authoring.health
            });
        }
    }
}
