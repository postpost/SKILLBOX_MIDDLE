using Unity.Entities;
using UnityEngine;

public class HealthPackAuth : MonoBehaviour
{
    public float healthAmount = 10f;
    public float radius = 1.5f;

    public class HealthPackBaker : Baker<HealthPackAuth>
    {
        public override void Bake(HealthPackAuth authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HealthPackData
            {
                HealthAmount = authoring.healthAmount,
                Radius = authoring.radius,
            });
            AddComponent<HealthPackTag>(entity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, radius);  

    }
}
