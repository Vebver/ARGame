using UnityEngine;

public class CrystalHealth : MonoBehaviour
{
    public float currentHealth = 1000f;
    public GameObject gameOverUI;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Crystal Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Crystal Destroyed! Game Over.");
        // Optionally, show a game over screen or restart the level
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        Time.timeScale = 0f; // Pause the game
    }
}

