using Unity.Entities;
using UnityEngine;

public class FireProjectileAuthoring : MonoBehaviour
{
    //pass prefab here
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

    //create Baker
    public class FireProjectileBaker : Baker<FireProjectileAuthoring>
    {
        public override void Bake(FireProjectileAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            // ПРЕВРАЩАЕМ Transform в Entity
            var shootPointEntity = GetEntity(authoring.shootPoint, TransformUsageFlags.Dynamic) ;
            AddComponent(entity, new ProjectilePrefab
            {
                Value = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic),
                ShootPointEntity = shootPointEntity
            });
            AddComponent<FireProjectileTag>(entity);

            // По умолчанию выключаем тег, чтобы не стреляло само при старте
            SetComponentEnabled<FireProjectileTag>(entity, false);
        }
    }
}

public struct ProjectilePrefab: IComponentData
{
    public Entity Value; //здесь сам префаб
    public Entity ShootPointEntity; //точка выстрела
}

public struct FireProjectileTag : IComponentData, IEnableableComponent
{
    //IEnableableComponent чтобы вкл или выкл компонент в системе
}
