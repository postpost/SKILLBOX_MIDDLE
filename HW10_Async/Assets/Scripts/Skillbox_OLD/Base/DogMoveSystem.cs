using System.Diagnostics;
using Unity.Entities;
using Unity.Mathematics; // Для float3
using Unity.Transforms; // Для LocalTransform
using UnityEngine; // Add this using directive for Debug.Log

public partial class DogMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // 1. Берем deltaTime через SystemAPI
        float deltaTime = SystemAPI.Time.DeltaTime;

        // 2. Используем современный Query. 
        // Он автоматически найдет всех, у кого есть LocalTransform и DogMoveComponent
        foreach (var (transform, dogMove) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<DogMoveComponent>>())
        {
            // 3. В ECS мы работаем с float3, а не Vector3
            // dogMove.ValueRO.Speed берется из вашей структуры IComponentData
            transform.ValueRW.Position.y += dogMove.ValueRO.Speed * deltaTime;
            UnityEngine.Debug.Log("Система заработала");

        }
    }
}
