using UnityEngine;

public class DarkCrystalHealth : MonoBehaviour
{
    public float currentHealth = 100f;
    public int spawnPointIndex; // Assign this in Inspector to link to a specific spawn point

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Dark Crystal " + spawnPointIndex + " Health: " + currentHealth);

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

