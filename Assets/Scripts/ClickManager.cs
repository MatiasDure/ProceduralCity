using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Singleton = null;
    private UpdateBuilding _selected;

    public UpdateBuilding SelectedBuilding => _selected;

    public static event Action<bool> OnBuildingSelected;

    private void Awake()
    {
        if(Singleton == null) Singleton = this;
        if(Singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider == null)
                {
                    UpdateSelected(null);
                    return;
                }

                IClickable clickableObj = hitInfo.collider.gameObject.GetComponent<IClickable>();

                if (clickableObj == null) return;

                //clickableObj.HandleClick();

                if (hitInfo.collider.gameObject.TryGetComponent(out UpdateBuilding component)) UpdateSelected(component);
            }
            else UpdateSelected(null);
        }
    }

    private void UpdateSelected(UpdateBuilding pNewBuildingSelected)
    {
        _selected = pNewBuildingSelected;
        OnBuildingSelected?.Invoke(pNewBuildingSelected != null);
    }
}
