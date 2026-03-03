using System.Collections.Generic;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTakerAuthoring : MonoBehaviour, IAbilityTarget
{
    public int damage;
    public List<GameObject> Targets { get; set; } = new List<GameObject>();

    private EntityManager cachedEm;
    private bool isWorldFound;
    private HealthData healthData;
    private Entity targetEntity;
    public void Execute()
    {
        FindEntity();
        ApplyDamage();
    }

    private void FindEntity()
    {
        if (Targets.Count == 0 || Targets == null) return;
        foreach (var target in Targets)
        {
            if (target.TryGetComponent<CharacterHealthAuthoring>(out var healthAuth))
            {
                targetEntity = healthAuth.selfRef;
                return;
            }
            else
                Debug.Log("Entity was not found");
        }
    }

    //негибко
    private void ApplyDamage()
    {
        if (targetEntity == Entity.Null) return;
        foreach(World world in World.All)
        {
            EntityManager em = world.EntityManager;
            if (em.Exists(targetEntity) && em.HasComponent<HealthData>(targetEntity)) //сущность в одном мире HealthData в другом
            {
                healthData = em.GetComponentData<HealthData>(targetEntity);
                healthData.Value -= damage;
                em.SetComponentData(targetEntity, healthData);
                return;
            }
            else
                Debug.Log($"entity exists: {em.Exists(targetEntity)}; has component: {em.HasComponent<HealthData>(targetEntity)}");
        }
    }

     public class DamageTakerBaker : Baker<DamageTakerAuthoring>
    {
        public override void Bake(DamageTakerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, authoring); //нужна ссылка, чтобы находить именно у этой сущности 
            AddComponent(entity, new DamageTakerData
            {
                Value = authoring.damage
            });
        }
    }

    public void ExecuteAll(Collider[] hits, int count)
    {
        
    }
}

public struct DamageTakerData: IComponentData
{
    public float Value;
}
