using Unity.Entities;
using UnityEngine;

public class FreezeCapabilityManager : MonoBehaviour
{
    public Material freezeMaterial;
    public float freezeActivationTime;
    public float totalFreezeDuration;
}

public class FreezeCapabilityBaker : Baker<FreezeCapabilityManager>
{
    
    public override void Bake(FreezeCapabilityManager authoring)
    {
        int freezePropertyID = Shader.PropertyToID("_FreezeAmount");
        authoring.freezeMaterial.SetFloat(freezePropertyID, 0f);
        
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponentObject(entity, new FreezeCapabilityData
        {
            FreezeMaterial = authoring.freezeMaterial,
            FreezeActivationTime = authoring.freezeActivationTime,
            TotalFreezeDuration = authoring.totalFreezeDuration,
        });

        AddComponent<FreezeTag>(entity);
        SetComponentEnabled<FreezeTag>(entity, false);
    }
}
