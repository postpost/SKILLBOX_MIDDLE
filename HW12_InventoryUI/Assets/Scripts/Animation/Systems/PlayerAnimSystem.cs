using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using DG.Tweening;
using Unity.VisualScripting;

//идея в том, чтобы вытащить Animator из префаба и управлять им в сцене
[UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)] //обновление визуала и аним в конце кадра после логики и физики
public partial struct PlayerAnimSystem : ISystem
{
    Entity playerEntity;
    float elapsedTime;
    bool isDamaged;
    bool isDead;
    
    public void OnUpdate(ref SystemState state)
    {
        //Move animation
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach(var (playerVisualPrefab, entity) in SystemAPI.Query<PlayerVisualPrefabData>().
                                                    WithNone<AnimatorReference>().
                                                    WithEntityAccess())
        {
            playerEntity = entity;
            //создадим визуальную составляющую на сцене
            var newComponentObj = Object.Instantiate(playerVisualPrefab.Value);
            var newAnimatorReference = new AnimatorReference
            {
                Value = newComponentObj.GetComponent<Animator>()
            };

            ecb.AddComponent(entity, newAnimatorReference);
        }
        //синхронизируем трансформ с анимацией
        foreach (var (transform, animator, input, animHash, healthData) in SystemAPI.Query<RefRO<LocalTransform>,
                                                    AnimatorReference,
                                                    RefRO<PlayerMoveInput>,
                                                    RefRO<AnimData>, 
                                                    RefRO<HealthData>>())
        {
            //Move
            var speedsq = math.lengthsq(input.ValueRO.Value);
            bool isMoving = speedsq > 0.001f;
            animator.Value.SetBool(animHash.ValueRO.MoveHash, isMoving);

            //Damage
            
            if (playerEntity != Entity.Null && state.EntityManager.IsComponentEnabled<GetHit>(playerEntity))
            {
                isDamaged = true;
                SystemAPI.SetComponentEnabled<GetHit>(playerEntity, false);
            }

            TickToDamage(ref state);

            //Death
            if(healthData.ValueRO.Value <=0 && !isDead)
            {
                isDead = true;
                isDamaged = false;
                SystemAPI.SetComponentEnabled<DeathTag>(playerEntity, isDead);
                animator.Value.SetTrigger(animHash.ValueRO.DeadAnimHash);
                Debug.Log("<color=blue>IS DEAD </color>");
            }

            animator.Value.SetBool(animHash.ValueRO.Damagedhash, isDamaged);

            animator.Value.transform.position = transform.ValueRO.Position;
            animator.Value.transform.rotation = transform.ValueRO.Rotation;
        }
        ecb.Playback(state.EntityManager); //исполнить все команды
        ecb.Dispose(); //очистить память
    }

    //выключает isDamaged через какое-то время
    private void TickToDamage(ref SystemState state)
    {
        if (!isDamaged || isDead) return;
        
        elapsedTime += SystemAPI.Time.DeltaTime;
        
        if (elapsedTime >= 2f)
        {
            isDamaged = false;
            elapsedTime = 0;
        }
    }
}
