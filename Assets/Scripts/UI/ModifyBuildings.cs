using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyBuildings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ClickManager.OnBuildingSelected += ActivateChildren;
        ActivateChildren(false);
    }

    private void ActivateChildren(bool pActivate)
    {
        for (int i = 0; transform.childCount > i; i++)
        {
            transform.GetChild(i).gameObject.SetActive(pActivate);
        }
    }

    private void OnDestroy()
    {
        ClickManager.OnBuildingSelected -= ActivateChildren;
    }
}
