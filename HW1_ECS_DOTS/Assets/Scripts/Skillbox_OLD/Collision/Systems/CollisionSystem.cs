using System;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static CollisionAbilityAuthoring;

public partial class CollisionSystem : SystemBase
{
    // Add a Collider[] array for use with OverlapSphereNonAlloc
    private Collider[] colliderResults;

    protected override void OnCreate()
    {
        // Allocate Collider[] for OverlapSphereNonAlloc
        colliderResults = new Collider[50];
    }

    protected override void OnUpdate()
    {
        foreach (var (transform, colliderData, abilityCollision, entity) in
                                SystemAPI.Query<RefRO<LocalTransform>,
                                RefRO<CollisionAbilityAuthoring.ColliderData>, CollisionAbilityReference>()
                                .WithEntityAccess())
        {
            float3 position = transform.ValueRO.Position;
            quaternion rotation = transform.ValueRO.Rotation;

            int size = 0;

            switch (colliderData.ValueRO.ColliderType)
            {
                case ColliderType.Sphere:
                    Vector3 sphereCenter = (Vector3)(colliderData.ValueRO.SphereCenter + position);
                    size = Physics.OverlapSphereNonAlloc(
                                    sphereCenter,
                                    colliderData.ValueRO.SphereRadius,
                                    colliderResults
                    );
                    break;

                case ColliderType.Capsule:
                    float3 point0 = colliderData.ValueRO.CapsuleStart + position;
                    float3 point1 = colliderData.ValueRO.CapsuleEnd + position;
                    float3 center = (point0 + point1) * 0.5f;

                    point0 = center + math.mul(rotation, (point0 - center));
                    point1 = center + math.mul(rotation, (point1 - center));
                    size = Physics.OverlapCapsuleNonAlloc(
                                    (Vector3)point0, 
                                    (Vector3)point1, 
                                    colliderData.ValueRO.CapsuleRadius, 
                                    colliderResults);
                    break;

                case ColliderType.Box:
                    Vector3 boxCenter = (Vector3)(colliderData.ValueRO.BoxCenter + position);
                    size = Physics.OverlapBoxNonAlloc(
                                    boxCenter,
                                    colliderData.ValueRO.BoxHalfExtents, 
                                    colliderResults, 
                                    rotation);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if(size > 0)
            {
                abilityCollision.Value.Value.Execute();
                for (int i = 0; i < size; i++)
                {
                    Collider hit = colliderResults[i];
                    //Debug.Log($"Entity {entity.Index} hits {hit.name}");
                }

                if (SystemAPI.HasComponent<CollisionAbilityReference>(entity) && abilityCollision.Value.Value != null)
                {
                    //Debug.Log($"система передала коллайдеры в сущность CollisionAbility размером: {size}");
                    abilityCollision.Value.Value.ExecuteAll(colliderResults, size);
                }
            }
        }
    }
}
