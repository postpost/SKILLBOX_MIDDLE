using Unity.Entities;
using UnityEngine;

public partial class FreezeVisualSystem : SystemBase
{
    float elapsedTime = 0f;
    int freezeAmountID;
    protected override void OnCreate()
    {
        freezeAmountID = Shader.PropertyToID("_FreezeAmount");
    }
    protected override void OnUpdate()
    {
        foreach(var (freezeData, entity) in SystemAPI.Query<FreezeCapabilityData>().
                                            WithAll<FreezeTag>().
                                            WithEntityAccess())
        {
            float freezeAmount = Mathf.Clamp01(elapsedTime / (freezeData.FreezeActivationTime + 0.001f));
            elapsedTime += SystemAPI.Time.DeltaTime;
            freezeData.FreezeMaterial.SetFloat(freezeAmountID, freezeAmount);
            float freezeCancelTime = Mathf.Clamp01(elapsedTime / freezeData.TotalFreezeDuration + 0.001f);
           
            if(freezeAmount >= 1f && freezeCancelTime >=1)
            {
                elapsedTime = 0;
                freezeData.FreezeMaterial.SetFloat(freezeAmountID, 0f);
                SystemAPI.SetComponentEnabled<FreezeTag>(entity, false);
            }
        }
    }
}