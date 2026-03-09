using Unity.Entities;
using UnityEngine;

public partial class FreezeVisualSystem : SystemBase
{
    int freezeAmountID;
    protected override void OnCreate()
    {
        freezeAmountID = Shader.PropertyToID("_FreezeAmount");
    }
    protected override void OnUpdate()
    {
        float dt = SystemAPI.Time.DeltaTime;
        foreach(var (freezeData, entity) in SystemAPI.Query<FreezeCapabilityData>().
                                            WithAll<FreezeTag>().
                                            WithEntityAccess())
        {
            freezeData.CurrentTime += dt;
            float freezeAmount = Mathf.Clamp01(freezeData.CurrentTime / (freezeData.FreezeActivationTime + 0.001f));
          
            freezeData.FreezeMaterial.SetFloat(freezeAmountID, freezeAmount);
                      
            if(freezeData.CurrentTime >= freezeData.TotalFreezeDuration)
            {
                freezeData.CurrentTime = 0;
                freezeData.FreezeMaterial.SetFloat(freezeAmountID, 0f);
                SystemAPI.SetComponentEnabled<FreezeTag>(entity, false);
            }
        }
    }
}