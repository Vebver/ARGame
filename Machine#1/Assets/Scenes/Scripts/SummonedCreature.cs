using UnityEngine;

public class SummonedCreature : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int damage = 5;
    public int maxHealth = 50;
    private Animator animator;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GameObject target = FindNearestEnemy();
        if (target != null)
        {
            MoveTowards(target);
            if (Vector3.Distance(transform.position, target.transform.position) < 1f)
            {
                Attack(target);
            }

            if (animator != null)
                animator.SetBool("IsWalking", true);

            // ✅ If close enough, attack
            if (Vector3.Distance(transform.position, target.transform.position) < 1f)
            {
                Attack(target);
            }
        }

        else
        {
            // No target → stop walking
            if (animator != null)
                animator.SetBool("IsWalking", false);
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }

    void MoveTowards(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void Attack(GameObject enemy)
    {
        if (animator != null)
            animator.SetTrigger("Attack");

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
            if (animator != null)
            {
                animator.SetTrigger("Death");
            }

            Destroy(gameObject);
        }
    }
}