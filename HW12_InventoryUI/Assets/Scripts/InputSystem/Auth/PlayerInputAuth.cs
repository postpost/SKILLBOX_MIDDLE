using System;
using Unity.Entities;
using UnityEngine;

public class PlayerInputAuth : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float dashFactor;
 
    public class PlayerMoveBaker : Baker<PlayerInputAuth>
    {
        public override void Bake(PlayerInputAuth auth)
        {
            var entity = GetEntity(auth, TransformUsageFlags.Dynamic);
            AddComponent<PlayerMoveInput>(entity);
            AddComponent<PlayerTag>(entity);
            AddComponent(entity, new PlayerMoveSpeed { Value = auth.speed });
            AddComponent<PlayerDashTag>(entity);
            AddComponent(entity, new PlayerDashFactor { Value = auth.dashFactor });

            //чтобы система не сделала рывок до нажатия клавиши
            SetComponentEnabled<PlayerDashTag>(entity, false);
        }
    }
}
