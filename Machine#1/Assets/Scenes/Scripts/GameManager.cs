using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Energy System")]
    public int currentEnergy = 100;
    public int maxEnergy = 200;
    public float energyRechargeRate = 5f; // Energy per second
    public int creatureEnergyCost = 3;

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

    public void AddEnergy(int amount)
    {
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
        Debug.Log("Energy Gained: " + amount + ", Current Energy: " + currentEnergy);
    }

    public void OnEnemyKilled(Vector3 enemyPosition)
    {
        AddEnergy(5);

    }

    public bool TrySpendEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            Debug.Log("Energy Spent: " + amount + ", Remaining Energy: " + currentEnergy);
            return true;
        }
        return false;
    }
}

