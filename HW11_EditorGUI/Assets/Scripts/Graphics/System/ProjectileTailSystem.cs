using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(ProjectileMoveSystem))] //чтобы точно после обновления всех координат
public partial class ProjectileTailSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (GlobalVFX.Instance == null) return;

        //pick all ProjectileVisualTag and send their position to the vfx system
        foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<ProjectileVisualTag>())
        {
            GlobalVFX.Instance.SendData(transform.ValueRO.Position);
            //Debug.Log($"Sent {transform.ValueRO.Position} projectile positions to VFX system");
        }
    }
}
