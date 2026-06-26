using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public System.Action OnDeath; 

    public System.Action OnDamaged;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} получил урон, здоровье: {currentHealth}");

        OnDamaged?.Invoke();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} умер!");
        OnDeath?.Invoke();
        FindAnyObjectByType<restart>().ShowDeathMenu();
    }
}