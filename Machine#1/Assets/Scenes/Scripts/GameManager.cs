using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Energy System")]
    public int currentEnergy = 100;
    public int maxEnergy = 200;
    public float energyRechargeRate = 5f; // Energy per second

    private float lastEnergyRechargeTime;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        lastEnergyRechargeTime = Time.time;
    }

    void Update()
    {
        RechargeEnergy();
    }

    void RechargeEnergy()
    {
        if (Time.time >= lastEnergyRechargeTime + 1f)
        {
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy + Mathf.RoundToInt(energyRechargeRate));
            lastEnergyRechargeTime = Time.time;
            Debug.Log("Current Energy: " + currentEnergy);
        }
    }

    public bool CanAfford(int cost)
    {
        return currentEnergy >= cost;
    }

    public void SpendEnergy(int amount)
    {
        currentEnergy -= amount;
        Debug.Log("Energy Spent: " + amount + ", Remaining Energy: " + currentEnergy);
    }

    public void AddEnergy(int amount)
    {
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
        Debug.Log("Energy Gained: " + amount + ", Current Energy: " + currentEnergy);
    }

    public void OnEnemyKilled(Vector3 enemyPosition)
    {
        // This is called from EnemyAI when an enemy dies
        // Here you can implement logic for dropping energy orbs
        // For now, let's just add a small amount of energy directly
        AddEnergy(10); // Example: Add 10 energy per killed enemy

        // You could also instantiate an energy orb prefab here
        // GameObject energyOrb = Instantiate(energyOrbPrefab, enemyPosition, Quaternion.identity);
        // energyOrb.GetComponent<EnergyOrb>().Initialize(10); // Pass energy amount to orb
    }
}

