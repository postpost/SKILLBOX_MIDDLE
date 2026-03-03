using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public HealthData GetHealthData { get; set; }
    public GameObject InventoryUIRoot; //куда складывать префаб для инвенторя
    List<MonoBehaviour> levelupActions;
    private int currentLevel = 1;
    private int score = 0;
    private int scoreToNextLevel = 10;

    public int CurrentLevel => currentLevel;    
    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        if (score >= scoreToNextLevel) LevelUp();
    }

    private void LevelUp()
    {
        ++currentLevel;
        scoreToNextLevel *= 2;

        foreach (var action in levelupActions)
        {
            if(action is ILevelUp levelUp)
            {
                levelUp.LevelUp(this, currentLevel);
            }
        }
    }
}
