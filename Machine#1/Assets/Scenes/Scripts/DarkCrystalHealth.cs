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

        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopSpawningFromLane(spawnPointIndex);
        }

        // Notify manager
        CrystalManager manager = FindObjectOfType<CrystalManager>();
        if (manager != null)
        {
            manager.CheckCrystals();
        }

        // Destroy this crystal object if needed
        Destroy(gameObject);
    }

    // Helper method for the manager
    public bool IsAlive()
    {
        return currentHealth > 0;
    }

}
