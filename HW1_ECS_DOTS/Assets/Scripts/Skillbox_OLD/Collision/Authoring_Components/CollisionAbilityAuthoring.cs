using Assets.Scripts.InputSystem.ECS_NEW;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CollisionAbilityAuthoring : MonoBehaviour, IAbility
{
    public Collider Collider;

    public List<MonoBehaviour> collisionActions = new();
    public List<IAbilityTarget> abilityTargets = new();

    //private void Start() //не работает для сущностей в subscene (в игровом мире может быть только для физики или рендера)
    //{
    //    Debug.Log($"collisionActions count: {collisionActions.Count}");
       
    //}

    public virtual void ExecuteAll(Collider[] hits, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject go = hits[i].gameObject;
            foreach(var target in abilityTargets)
            {
                if(target.Targets.Contains(go)) continue;
                target.Targets.Clear();
                target.Targets.Add(go);
                target.Execute();
            }
        }
    }

    public virtual void Execute()
    {
        Debug.Log($"ability count: {abilityTargets.Count}");
        foreach(var target in abilityTargets)
        {
             target.Execute();
        }
    }

    private void AddAbilities()
    {
        foreach (var action in collisionActions)
        {
            if (action is IAbilityTarget ability)
                abilityTargets.Add(ability);
            else
                Debug.Log("Collision actions must be derived from the IAbility");
        }
    }

    public class CollisionAbilityBaker : Baker<CollisionAbilityAuthoring>
    {
        //это нам нужно для использования общего метода в дочерних классах
        public static void BakeBase(IBaker baker, CollisionAbilityAuthoring authoring)
        {
            float3 position = authoring.transform.position;
            //add colliders to our entity and find which colliders it has 
            var entity = baker.GetEntity(TransformUsageFlags.Dynamic);
            baker.AddComponent(entity, new CollisionAbilityReference
            {
                Value = authoring
            });

            switch (authoring.Collider)
            {
                case SphereCollider sphere:
                    sphere.ToWorldSpaceSphere(out var sphereCenter, out var sphereRadius);
                    baker.AddComponent(entity, new ColliderData
                    {
                        ColliderType = ColliderType.Sphere,
                        SphereCenter = sphereCenter - position, // get center local space for the entity
                        SphereRadius = sphereRadius,
                        InititalTakeOff = true
                    });
                    break;

                case CapsuleCollider capsule:
                    capsule.ToWorldSpaceCapsule(out var capsuleStart, out var capsuleEnd, out var capsuleRadius);
                    baker.AddComponent(entity, new ColliderData
                    {
                        ColliderType = ColliderType.Capsule,
                        CapsuleStart = capsuleStart - position,
                        CapsuleEnd = capsuleEnd - position,
                        CapsuleRadius = capsuleRadius,
                        InititalTakeOff = true
                    });
                    break;
                
                case BoxCollider box:
                    box.ToWorldSpaceBox(out var center, out var halfExtents, out var orientation);
                    baker.AddComponent(entity, new ColliderData
                    {
                        ColliderType = ColliderType.Box,
                        BoxCenter = center - position,
                        BoxHalfExtents = halfExtents,
                        BoxOrientation = orientation,
                        InititalTakeOff = true
                    });
                    break;
            }
            authoring.Collider.enabled = false; //выключаем коллизию с самой сущностью (с собой)
        }

        public override void Bake(CollisionAbilityAuthoring authoring)
        {
            authoring.AddAbilities();
            BakeBase(this, authoring);
        }
    }

    public struct CollisionAbilityReference: IComponentData
    {
        public UnityObjectRef<CollisionAbilityAuthoring> Value;
    }

    public struct ColliderData: IComponentData
    {
        public ColliderType ColliderType;
        public float3 SphereCenter;
        public float SphereRadius;

        public float3 CapsuleStart;
        public float3 CapsuleEnd;
        public float CapsuleRadius;

        public float3 BoxCenter;
        public float3 BoxHalfExtents;
        public quaternion BoxOrientation;
        public bool InititalTakeOff;

    }

    public enum ColliderType
    {
        Sphere = 0, 
        Capsule = 1,
        Box = 2,
    }
}
