using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct PlayerMoveInput : IComponentData
{
    public float2 Value;
}

public struct PlayerMoveSpeed: IComponentData
{
    public float Value;
}

public struct PlayerTag : IComponentData { }
public struct PlayerDashFactor:IComponentData
{
    public float Value;
}

public struct PlayerDashTag : IComponentData, IEnableableComponent { }
