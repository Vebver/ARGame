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
    private GameObject currentTarget;
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
        if (isDead) return;

        // Find the best target each frame
        currentTarget = FindBestTarget();

        if (currentTarget == null) return;

        // Move towards target
        if (navAgent != null && navAgent.enabled)
        {
            navAgent.SetDestination(currentTarget.transform.position);

            bool isMoving = navAgent.velocity.magnitude > 0.1f;
            if (animator != null)
            {
                animator.SetBool("IsWalking", isMoving);
            }

            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (distanceToTarget <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackTarget();
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

    GameObject FindBestTarget()
    {
        // Get all possible targets
        GameObject crystal = GameObject.FindWithTag("Crystal");
        GameObject[] towers = GameObject.FindGameObjectsWithTag("DefenseTower");
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("SummonedCreature");

        // Combine all targets into a list
        var targets = new System.Collections.Generic.List<GameObject>();
        if (crystal != null) targets.Add(crystal);
        targets.AddRange(towers);
        targets.AddRange(creatures);

        // Find the closest target
        GameObject best = null;
        float minDist = Mathf.Infinity;
        foreach (var t in targets)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                best = t;
            }
        }
        return best;
    }

    void AttackTarget()
    {
        lastAttackTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Damage the target based on its type
        if (currentTarget.CompareTag("Crystal"))
        {
            CrystalHealth crystalHealth = currentTarget.GetComponent<CrystalHealth>();
            if (crystalHealth != null)
            {
                crystalHealth.TakeDamage(attackDamage);
            }
        }
        else if (currentTarget.CompareTag("DefenseTower"))
        {
            DefenseTower tower = currentTarget.GetComponent<DefenseTower>();
            if (tower != null)
            {
                tower.TakeDamage((int)attackDamage);
            }
        }
        else if (currentTarget.CompareTag("SummonedCreature"))
        {
            SummonedCreature creature = currentTarget.GetComponent<SummonedCreature>();
            if (creature != null)
            {
                creature.TakeDamage((int)attackDamage);
            }
        }
    }
}

