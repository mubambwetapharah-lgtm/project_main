using UnityEngine;
using UnityEngine.UI;

public class HP_UI : MonoBehaviour
{
    [Header("Игроки")]
    public PlayerHealth player1;
    public PlayerHealth player2;

    [Header("Полоски")]
    public Slider healthBar1;
    public Slider healthBar2;

    void Start()
    {
        healthBar1.maxValue = player1.maxHealth;
        healthBar2.maxValue = player2.maxHealth;
    }

    void Update()
    {
        healthBar1.value = player1.currentHealth;
        healthBar2.value = player2.currentHealth;
    }
}