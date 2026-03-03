using Unity.Collections;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class TrapAbilityAuthoring : CollisionAbilityAuthoring
{
    public int damage;
    public float radius;

    public override void ExecuteAll(Collider[] colliders, int count)
    {
        ////можем сохранить сюда или что-то сделать еще с этими данными
        // получаем ссылку на мир ECS
              
        for (int i = 0; i < count; ++i)
        {
            //сначала получаем ссылку на сущность
            if (colliders[i].TryGetComponent<CharacterHealthAuthoring>(out var targetEntity))
            {
                //Debug.Log($"{targetEntity.name}. Entity ID: {targetEntity.selfRef.Index}");
                Entity target = targetEntity.selfRef;

                foreach(var world in World.All)
                {
                    EntityManager em = world.EntityManager;
                    if(em.Exists(target) && em.HasComponent<HealthData>(target))
                    {
                        var healthData = em.GetComponentData<HealthData>(target);
                        healthData.Value -= damage;
                        em.SetComponentData(target, healthData);
                        Debug.Log($"Здоровье: {healthData.Value}");
                    }
                    else
                        Debug.Log("Ловушка перерыла все миры, но HealthData не нашла");
                }
            }

        }
    }

    public class TrapAbilityBaker : Baker<TrapAbilityAuthoring>
    {
        public override void Bake(TrapAbilityAuthoring authoring)
        {
            //привязыаем общий метод с коллизиями
            CollisionAbilityBaker.BakeBase(this, authoring);
            //свой внутренний бейкер
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TrapData
            {
                Damage = authoring.damage,
                Radius = authoring.radius
                
            });
        }
    }
}
