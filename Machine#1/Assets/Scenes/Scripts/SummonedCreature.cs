using UnityEngine;

public class SummonedCreature : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int damage = 1;
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
        GameObject target = FindNearestTarget();
        if (target != null)
        {
            MoveTowards(target);
            if (Vector3.Distance(transform.position, target.transform.position) < 1f)
            {
                Attack(target);
            }

            if (animator != null)
                animator.SetBool("IsWalking", true);
        }
         else
        {
            if (animator != null)
                animator.SetBool("IsWalking", false);
        }
    }

    GameObject FindNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] crystals = GameObject.FindGameObjectsWithTag("DarkCrystal");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in enemies)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = obj;
            }
        }
        foreach (GameObject obj in crystals)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = obj;
            }
        }
        return nearest;
    }

    void MoveTowards(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void Attack(GameObject target)
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        EnemyAI enemyAI = target.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.TakeDamage(damage);
            return;
        }

        // Try to damage a dark crystal
        DarkCrystalHealth darkCrystal = target.GetComponent<DarkCrystalHealth>();
        if (darkCrystal != null)
        {
            darkCrystal.TakeDamage((float)damage);
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