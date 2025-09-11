using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DarkCrystalHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 2000f;
    public Healthbar healthbar; // Reference to your Healthbar script

    public int spawnPointIndex;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.UpdateHealthbar(maxHealth, currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        healthbar.UpdateHealthbar(maxHealth, currentHealth);
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
        SceneManager.LoadScene("Winner");
    }
}
