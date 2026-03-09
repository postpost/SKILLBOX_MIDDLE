using Unity.Entities;
using UnityEngine;

public class AnimatorAuth : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private string moveAnimHash;
    [SerializeField] private string damagedAnimHash;
    [SerializeField] private string deadAnimHash;
    [SerializeField] private GameObject playerVisualPrefab;

    public class AnimBaker : Baker<AnimatorAuth>
    {
        public override void Bake(AnimatorAuth auth)
        {
            Entity entity = GetEntity(auth, TransformUsageFlags.Dynamic);
            
             if(auth.playerVisualPrefab != null)
            {
                AddComponentObject(entity, new PlayerVisualPrefabData
                {
                    Value = auth.playerVisualPrefab   
                });


                AddComponent(entity, new AnimData
                {
                    MoveHash = Animator.StringToHash(auth.moveAnimHash),
                    Damagedhash = Animator.StringToHash(auth.damagedAnimHash),
                    DeadAnimHash = Animator.StringToHash(auth.deadAnimHash)
                });
            }

            else
            {
                Debug.Log("Player Visual Prefab is empty!");
            }
        }
    }
}
