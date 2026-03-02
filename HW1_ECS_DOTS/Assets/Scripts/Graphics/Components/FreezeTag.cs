using Unity.Entities;
using UnityEngine;

public struct FreezeTag : IComponentData, IEnableableComponent
{
}
public class FreezeCapabilityData:IComponentData
{
    public Material FreezeMaterial;
    public float FreezeActivationTime;
    public float TotalFreezeDuration;
}
