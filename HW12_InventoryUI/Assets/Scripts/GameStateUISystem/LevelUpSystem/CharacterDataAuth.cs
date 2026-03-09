using Unity.Entities;
using UnityEngine;

public class CharacterDataAuth : MonoBehaviour
{
    public int currentLevel = 1;
    public int score = 0;
    public int scoreToNextLevel = 20;
    public int minLevel = 0;

    public class CharacterDataBaker : Baker<CharacterDataAuth>
    {
        public override void Bake(CharacterDataAuth authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterDataComponent
            {
                CurrentLevel = authoring.currentLevel,
                Score = authoring.score,
                ScoreToNextLevel = authoring.scoreToNextLevel,
                MinLevel = authoring.minLevel,
            });

            AddBuffer<InventoryBufferElement>(entity);
        }
    }

}
public struct CharacterDataComponent: IComponentData
{
        public int CurrentLevel;
        public int Score;
        public int ScoreToNextLevel;
        public int MinLevel;
}
