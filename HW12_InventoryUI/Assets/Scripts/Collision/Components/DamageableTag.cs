using Unity.Entities;

public struct DamageableTag : IComponentData { }

public struct GetHit : IComponentData, IEnableableComponent { }

public struct DeathTag: IComponentData, IEnableableComponent { }
