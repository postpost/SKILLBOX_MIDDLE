using Unity.Entities;
using UnityEngine;
public struct AnimData : IComponentData
{
    public int MoveHash;
    public int Damagedhash;
    public int DeadAnimHash;
}

public class AnimatorReference: ICleanupComponentData
{
    public Animator Value;
}

public class PlayerVisualPrefabData:IComponentData
{
    public GameObject Value;
}
