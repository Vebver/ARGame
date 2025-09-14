using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalManager : MonoBehaviour
{
    private DarkCrystalHealth[] crystals;

    void Start()
    {
        crystals = FindObjectsOfType<DarkCrystalHealth>();
    }

    public void CheckCrystals()
    {
        foreach (DarkCrystalHealth crystal in crystals)
        {
            if (crystal != null && crystal.IsAlive())
            {
                return; // At least one crystal is still alive → don't load scene yet
            }
        }

        // If we get here, all crystals are destroyed
        SceneManager.LoadScene("Winner");
    }
}
