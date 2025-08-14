using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Summoning Creatures")]
    public GameObject creaturePrefab; // Assign your creature prefab in the Inspector

    public GameObject towerPrefab;
    [Header("Attack Settings")]
    public float attackDamage = 50f;
    public float attackRange = 10f;
    public GameObject attackEffect; // Optional particle effect for attacks
    
    [Header("Building/Summoning")]
    public LayerMask groundLayer = 1; // Layer for valid placement areas
    public Material previewMaterial; // Material for showing placement preview
    
    private Camera arCamera;
    private GameManager gameManager;
    private GameObject currentPreview; // Preview object for placement
    private bool isBuildingMode = false;
    private GameObject selectedBuildingPrefab;
    
    void Start()
    {
        arCamera = Camera.main; // Assuming AR Camera is tagged as MainCamera
        if (arCamera == null)
        {
            arCamera = FindObjectOfType<Camera>();
        }
        
        gameManager = FindObjectOfType<GameManager>();
    }
    
    void Update()
    {
        HandleInput();
        
        if (isBuildingMode)
        {
            UpdateBuildingPreview();
        }
    }
    
    void HandleInput()
    {
        // Handle touch input for mobile devices
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                if (isBuildingMode)
                {
                    TryPlaceBuilding(touch.position);
                }
                else
                {
                    TryAttackEnemy(touch.position);
                }
            }
        }
        
        // Handle mouse input for testing in editor
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 inputPosition = Input.mousePosition;
            
            if (isBuildingMode)
            {
                TryPlaceBuilding(inputPosition);
            }
            else
            {
                TryAttackEnemy(inputPosition);
            }
        }
    }
    
    void TryAttackEnemy(Vector2 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            // Check if we hit an enemy
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                // Attack the enemy
                enemy.TakeDamage(attackDamage);
                
                // Show attack effect
                if (attackEffect != null)
                {
                    GameObject effect = Instantiate(attackEffect, hit.point, Quaternion.identity);
                    Destroy(effect, 2f);
                }
                
                Debug.Log("Attacked enemy for " + attackDamage + " damage!");
                return; // Exit if an enemy was hit
            }

            // Check if we hit a dark crystal
            DarkCrystalHealth darkCrystal = hit.collider.GetComponent<DarkCrystalHealth>();
            if (darkCrystal != null)
            {
                darkCrystal.TakeDamage(attackDamage);
                if (attackEffect != null)
                {
                    GameObject effect = Instantiate(attackEffect, hit.point, Quaternion.identity);
                    Destroy(effect, 2f);
            }
                Debug.Log("Attacked dark crystal for " + attackDamage + " damage!");
        }
    }
    }
    

    void TryPlaceBuilding(Vector2 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // Check if we have enough energy
            if (gameManager != null && gameManager.CanAfford(GetBuildingCost()))
            {
                // Place the building
                GameObject building = Instantiate(selectedBuildingPrefab, hit.point, Quaternion.identity);
                
                // Deduct energy cost
                gameManager.SpendEnergy(GetBuildingCost());
                
                // Destroy only the preview, not the placed building
                if (currentPreview != null)
                {
                    Destroy(currentPreview);
                    currentPreview = null;
                }

                // Exit building mode
                ExitBuildingMode();
                
                Debug.Log("Building placed at " + hit.point);
            }
            else
            {
                Debug.Log("Not enough energy to build!");
            }
        }
    }
    
    void UpdateBuildingPreview()
    {
        if (currentPreview == null || selectedBuildingPrefab == null) return;
        
        // Cast ray from center of screen to show preview
        Ray ray = arCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            currentPreview.transform.position = hit.point;
            currentPreview.SetActive(true);
        }
        else
        {
            currentPreview.SetActive(false);
        }
    }
    
    public void EnterBuildingMode(GameObject buildingPrefab)
    {
        isBuildingMode = true;
        selectedBuildingPrefab = buildingPrefab;
        
        // Create preview object
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
        
        currentPreview = Instantiate(buildingPrefab);
        
        // Make it a preview (disable colliders, change material, etc.)
        Collider[] colliders = currentPreview.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        
        // Change material to preview material
        if (previewMaterial != null)
        {
            Renderer[] renderers = currentPreview.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = previewMaterial;
            }
        }
        
        Debug.Log("Entered building mode");
    }
    
    public void ExitBuildingMode()
    {
        isBuildingMode = false;
        selectedBuildingPrefab = null;
        
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
        
        Debug.Log("Exited building mode");
    }
    
    int GetBuildingCost()
    {
        // This should be configurable per building type
        // For now, return a default cost
        return 50;
    }
    public void SummonTower()
    {
        if (arCamera == null)
        {
            arCamera = Camera.main;
        }

        Vector3 spawnPosition = arCamera.transform.position + arCamera.transform.forward * 2f;
        Quaternion spawnRotation = Quaternion.identity;

        GameObject tower = Instantiate(towerPrefab, spawnPosition, spawnRotation);
        Debug.Log("Tower summoned at: " + tower.transform.position);
    }
    public void SummonCreature()
    {
        if (arCamera == null)
        {
            arCamera = Camera.main;
        }

        if (creaturePrefab == null)
        {
            Debug.LogWarning("No creature prefab assigned.");
            return;
        }

        // Check energy cost
        if (gameManager != null && !gameManager.CanAfford(GetCreatureCost()))
        {
            Debug.Log("Not enough energy to summon creature!");
            return;
        }

        // Spawn position in front of camera
        Vector3 spawnPosition = arCamera.transform.position + arCamera.transform.forward * 2f;
        Quaternion spawnRotation = Quaternion.identity;

        // Instantiate creature
        GameObject creature = Instantiate(creaturePrefab, spawnPosition, spawnRotation);

        // Deduct energy
        gameManager.SpendEnergy(GetCreatureCost());

        Debug.Log("Creature summoned at: " + creature.transform.position);
    }
    int GetCreatureCost()
    {
        return 30; // Set your creature's energy cost here
    }

}

