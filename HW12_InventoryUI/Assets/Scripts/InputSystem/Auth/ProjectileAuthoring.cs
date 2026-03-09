using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{
    [SerializeField] private float speed;
    public class ProjectileBaker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectileMoveSpeed
            {
                Value = authoring.speed
            });

            AddComponent<ProjectileVisualTag>(entity);
            
            //AddComponentObject(entity, new ParticleSystemReq
            //{
            //    ParticleSystem = authoring.GetComponent<ParticleSystem>()
            //});
        }
    }
}

public struct ProjectileMoveSpeed: IComponentData
{
    public float Value;
}

public struct ProjectileVisualTag : IComponentData
{

}

// Change ParticleSystemReq from struct to class so it can be used with AddComponentObject
public class ParticleSystemReq : IComponentData
{
    public ParticleSystem ParticleSystem;
}