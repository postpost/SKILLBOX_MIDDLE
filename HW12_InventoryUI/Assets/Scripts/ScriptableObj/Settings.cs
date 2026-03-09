using UnityEngine;

[CreateAssetMenu(fileName = "Settings")]
public class Settings : ScriptableObject, IHealthSettings
{
    public int characterMaxHealth = 150;

    public int MaxHealth => characterMaxHealth;
}
