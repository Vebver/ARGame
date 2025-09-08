using UnityEngine;
using UnityEngine.UI;

public class DarkCrystalHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 2000f;
    [SerializeField] private Healthbar _healthbar; // Fixed type name (capital B)

    public int spawnPointIndex;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health at start
        _healthbar.UpdateHealthbar(maxHealth, currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Dark Crystal " + spawnPointIndex + " Health: " + currentHealth);

        _healthbar.UpdateHealthbar(maxHealth, currentHealth); // Semicolon added

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Dark Crystal " + spawnPointIndex + " Destroyed!");

        // Notify the EnemySpawner to stop spawning from this lane
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopSpawningFromLane(spawnPointIndex);
        }

        // Optionally, play destruction effect or animation
        Destroy(gameObject);
    }
}
