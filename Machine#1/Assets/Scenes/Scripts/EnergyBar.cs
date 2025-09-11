using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField]
    private Image _energyBarSprite;

    // This method updates the UI based on the current energy
    public void UpdateEnergyBar(float maxEnergy, float currentEnergy)
    {
        _energyBarSprite.fillAmount = currentEnergy / maxEnergy;
    }
}