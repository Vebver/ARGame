using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 100f;
    public float attackDamage = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Drops")]
    public GameObject energyOrbPrefab; // Assign your EnergyOrb prefab here
    public float energyDropChance = 0.3f; // 30% chance to drop energy
    
    private NavMeshAgent navAgent;
    private Animator animator;
    private GameObject crystal;
    private float lastAttackTime;
    private bool isDead = false;
    
    void Start()
    {
        // Get components
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Find the crystal target
        crystal = GameObject.FindWithTag("Crystal");
        
        if (crystal == null)
        {
            Debug.LogError("Crystal not found! Make sure to tag your crystal GameObject as 'Crystal'");
        }
        
        // Set up NavMesh agent
        if (navAgent != null)
        {
            navAgent.speed = moveSpeed;
            navAgent.stoppingDistance = attackRange;
        }
    }
    
    void Update()
    {
        if (isDead || crystal == null) return;
        
        // Move towards crystal
        if (navAgent != null && navAgent.enabled)
        {
            navAgent.SetDestination(crystal.transform.position);
            
            // Update animation based on movement
            bool isMoving = navAgent.velocity.magnitude > 0.1f;
            if (animator != null)
            {
                animator.SetBool("IsWalking", isMoving);
            }
            
            // Check if close enough to attack
            float distanceToCrystal = Vector3.Distance(transform.position, crystal.transform.position);
            if (distanceToCrystal <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackCrystal();
            }
        }
    }
    
    void AttackCrystal()
    {
        lastAttackTime = Time.time;
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Damage the crystal
        CrystalHealth crystalHealth = crystal.GetComponent<CrystalHealth>();
        if (crystalHealth != null)
        {
            crystalHealth.TakeDamage(attackDamage);
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        health -= damage;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        isDead = true;
        
        // Disable movement
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }
        
        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        
        // Disable collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        // Drop energy orb (random chance)
        if (Random.Range(0f, 1f) < energyDropChance) 
        {
            DropEnergyOrb();
        }
        
        // Destroy after animation
        Destroy(gameObject, 3f);
    }
    
    void DropEnergyOrb()
    {
        if (energyOrbPrefab != null)
        {
            Instantiate(energyOrbPrefab, transform.position, Quaternion.identity);
        }
    }
}

