using UnityEngine;

public class StageCleared : MonoBehaviour
{
    [SerializeField] private GameObject moduleUpgradeUI;

    public void ContinueToModules()
    {
        moduleUpgradeUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
