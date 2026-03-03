using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class AIEvaluateSystem : SystemBase
{
    Entity playerEntity;
    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity)) return;
        var playerTransform = SystemAPI.GetComponent<LocalToWorld>(playerEntity);
        float3 playerPos = playerTransform.Position;

       foreach (var (agent, entity) in SystemAPI.Query<AIAgent>().WithEntityAccess())
        {
            if (agent == null || agent.IsBusy) continue;
            agent.Manager.StartInitializing();
            
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW.Position = agent.Manager.transform.position;
            transform.ValueRW.Rotation = agent.Manager.transform.rotation;
            //agent.Manager.Agent.destination = playerPos;
            agent.ActiveBehaviour = null;
            IBehaviour bestBehaviour; //если что удалить
            float highScore = float.MinValue;
            float currentScore = float.MinValue;
            foreach(var behavior in agent.Manager.behaviours)
            {
                if (behavior is IBehaviour ai)
                {
                    currentScore = ai.Evaluate((Vector3)playerPos);
                    //Debug.Log($"Evaluating {ai.GetType().Name} with score {currentScore}");
                    if (currentScore > highScore)
                    {
                        highScore = currentScore;
                        bestBehaviour = ai;
                        agent.ActiveBehaviour = bestBehaviour;
                    }
                }
            }
           // Debug.Log(agent.ActiveBehaviour);
        }
    }
}
