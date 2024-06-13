using System;
using UnityEngine;

public class BuildingTypesModifiers : MonoBehaviour
{
    [SerializeField] RadioButton _radioBtnPrefab;
    [SerializeField] Transform _contentContainer;

    // Start is called before the first frame update
    void Start()
    {
        BuildingType[] buildingTypes = (BuildingType[]) Enum.GetValues(typeof(BuildingType));

        foreach(BuildingType buildingType in buildingTypes)
        {
            CreateRadioButton(buildingType);
        }
    }

    private void CreateRadioButton(BuildingType buildingType)
    {
        RadioButton radioBtn = Instantiate(_radioBtnPrefab);
        radioBtn.SetRadioBtn(buildingType);
        radioBtn.transform.SetParent(_contentContainer);
    }
}
