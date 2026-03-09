using Unity.Entities;
using Unity.Mathematics;

public partial struct LevelUpSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach(var (characterData, entity) in SystemAPI.Query<RefRW<CharacterDataComponent>>()
                                                .WithEntityAccess())
        {
            if (characterData.ValueRO.Score >= characterData.ValueRO.ScoreToNextLevel)
            {
                //при переходе на сл уровень снова обнулим очки
                characterData.ValueRW.Score -= characterData.ValueRO.ScoreToNextLevel;
                characterData.ValueRW.CurrentLevel += 1;
                characterData.ValueRW.ScoreToNextLevel *= 2;
                if(SystemAPI.HasComponent<HealthData>(entity))
                {
                    var health = SystemAPI.GetComponentRW<HealthData>(entity);
                    characterData.ValueRW.MinLevel = 2;

                    if(characterData.ValueRO.CurrentLevel >= characterData.ValueRO.MinLevel)
                    {
                        health.ValueRW.Value += 30;
                        health.ValueRW.Value = math.min(health.ValueRW.Value, health.ValueRW.MaxHealth);
                    }
                }
            }
        }
    }
}
