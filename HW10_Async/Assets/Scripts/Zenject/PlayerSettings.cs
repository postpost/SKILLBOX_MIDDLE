using UnityEngine;

public class PlayerSettings : MonoBehaviour, IHealthSettings
{
    [SerializeField] private int characterMaxHealth = 100;

    public int MaxHealth => characterMaxHealth;
}
