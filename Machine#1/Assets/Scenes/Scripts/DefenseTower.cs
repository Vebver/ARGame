using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackCooldown = 1f;
    public int damage = 10;
    public int maxHealth = 100;

    private int currentHealth;
    private float lastAttackTime;

    void Start()
    {
        currentHealth = maxHealth;
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        GameObject target = FindNearestEnemy();
        if (target != null && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack(target);
            lastAttackTime = Time.time;
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = attackRange;
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }

    void Attack(GameObject enemy)
    {
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.TakeDamage(damage);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
} 