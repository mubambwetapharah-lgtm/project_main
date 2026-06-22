using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Players")]
    public PlayerHealth player1;
    public PlayerHealth player2;

    [Header("Health Bars")]
    public Slider healthBar1;
    public Slider healthBar2;

    void Start()
    {
        if (player1 != null && healthBar1 != null)
        {
            healthBar1.maxValue = player1.maxHealth;
            healthBar1.value = player1.currentHealth;
        }
        
        if (player2 != null && healthBar2 != null)
        {
            healthBar2.maxValue = player2.maxHealth;
            healthBar2.value = player2.currentHealth;
        }
    }

    void Update()
    {
        if (player1 != null && healthBar1 != null)
        {
            healthBar1.value = player1.currentHealth;
        }
        
        if (player2 != null && healthBar2 != null)
        {
            healthBar2.value = player2.currentHealth;
        }
    }
}