using Unity.Entities;
using UnityEngine;

public class CharacterHealthAuth : MonoBehaviour
{
    public class CharacterHealthAuthBaker : Baker<CharacterHealthAuth>
    {
        public override void Bake(CharacterHealthAuth authoring)
        {
            //if (authoring.settings == null) return;
           Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            //get ID to save/load data
            int persistenID = authoring.gameObject.GetHashCode();

            AddComponent(entity, new HealthData { 
                EntityID = persistenID,
                Value = 0,
                MaxHealth = 0
            });

            AddComponent<DamageableTag>(entity);
            AddComponent<GetHit>(entity);
            AddComponent<DeathTag>(entity);
            
            SetComponentEnabled<GetHit>(entity, false);
            SetComponentEnabled<DeathTag>(entity, false);
        }
    }
}
