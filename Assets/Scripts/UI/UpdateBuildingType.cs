using UnityEngine;

public class UpdateBuildingType : MonoBehaviour
{
    public void UpdateBuilding()
    {
        ClickManager.Singleton.SelectedBuilding.HandleClick();
    }
}
