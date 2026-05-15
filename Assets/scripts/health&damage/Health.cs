using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} получил урон, здоровье: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} умер!");
        // сюда позже подключим GameManager
    }
}