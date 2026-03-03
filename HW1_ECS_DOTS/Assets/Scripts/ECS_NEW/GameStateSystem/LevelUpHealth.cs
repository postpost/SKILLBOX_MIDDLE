using UnityEngine;

public class LevelUpHealth : MonoBehaviour, ILevelUp
{
    public int MinLevel { get; set; } 

    public void LevelUp(CharacterData data, int level) //возможно ли передать сюда ссылку на компонент здоровья
    {       
        if(data.CurrentLevel>MinLevel)
        {
            //можем здесь давать игроку тег HealthLevelUp через EntityManager
            //а система при таком теге увеличивает здоровье
            //IncreaseHealth();
            //увеличивает здоровье
            Debug.Log("health should be increased"); //будет делать система 
                                                     //var damageableQuery = SystemAPI.QueryBuilder().WithAll<LocalTransform, HealthData, DamageableTag>().Build();
        }
    }
}
