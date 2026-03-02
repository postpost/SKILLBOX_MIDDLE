using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public partial class AIBehaveSystem : SystemBase
{
    Entity player;
    protected override void OnUpdate()
    {
        if(!SystemAPI.TryGetSingletonEntity<PlayerTag>(out player)) return;
        float3 playerPos = SystemAPI.GetComponent<LocalToWorld>(player).Position;
        
        foreach (var agent in SystemAPI.Query<AIAgent>())
        {
            if(agent.IsBusy) continue;
            agent?.ActiveBehaviour?.Behave(playerPos);
        }
    }
}
