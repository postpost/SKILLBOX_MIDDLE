using Unity.Entities;

[System.Serializable] //for JSON
public struct HealthData : IComponentData
{
    public int EntityID; //for save/load
    public float Value;
    public float MaxHealth;
}
