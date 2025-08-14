using UnityEngine;

public class EnergyOrb : MonoBehaviour
{
    public int energyAmount = 25; // Amount of energy this orb provides
    public float collectionRadius = 2f; // Radius for player to collect the orb
    public float moveSpeed = 5f; // Speed at which orb moves towards player

    private Transform playerTransform; // Reference to the player (AR Camera)
    private bool collected = false;

    void Start()
    {
        // Find the AR Camera, which represents the player
        playerTransform = Camera.main.transform; 
        if (playerTransform == null)
        {
            Debug.LogError("AR Camera (Player) not found! Make sure your AR Camera is tagged as MainCamera.");
        }
    }

    void Update()
    {
        if (collected) return;

        // Check if player is within collection radius
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < collectionRadius)
        {
            collected = true;
            StartCoroutine(MoveToPlayer());
        }
    }

    System.Collections.IEnumerator MoveToPlayer()
    {
        while (Vector3.Distance(transform.position, playerTransform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Once close enough, add energy and destroy orb
        GameManager.Instance.AddEnergy(energyAmount);
        Destroy(gameObject);
    }

    // Optional: Draw gizmo in editor to visualize collection radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
}

