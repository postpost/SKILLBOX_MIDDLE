using Unity.Entities;
using UnityEngine;

public partial class CapabilityActivationSystem : SystemBase
{
    protected override void OnUpdate()
    {
    //    var deltaTime = SystemAPI.Time.DeltaTime;

    //    foreach (var (agent, freezeAmount, entity) in SystemAPI.Query<CapabilityAgent, RefRW<FreezeAmountData>>().WithAll<FreezeTag>().WithEntityAccess())
    //    {
    //        foreach (var capability in agent.Manager.capabilities)
    //        {
    //            if (capability is ICapable capable)
    //            {
    //                agent.ActiveCapability = capable;
    //                capable.Tick(deltaTime);

    //                freezeAmount.ValueRW.Value = Mathf.Clamp01(capable.ElapsedActiveTime / capable.TotalActivationTime);

    //                if (capable.isFinished)
    //                {
    //                    capable.Reset();
    //                    SystemAPI.SetComponentEnabled<FreezeTag>(entity, false);
    //                    agent.ActiveCapability = null;
    //                }
    //            }
    //        }
    //    }
    }
}
