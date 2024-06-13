using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadioButton : MonoBehaviour
{
    [SerializeField] private Button _radioBtn;
    [SerializeField] private TextMeshProUGUI _text;

    private BuildingType _buildingType;

    // Start is called before the first frame update
    void Start()
    {
        _radioBtn.onClick.AddListener(SelectRadioButton);
    }

    private void SelectRadioButton()
    {
        ClickManager.Singleton.SelectedBuilding.NextBuildingType = _buildingType;
    }

    public void SetRadioBtn(BuildingType pBuildingType)
    {
        _buildingType = pBuildingType;
        _text.text = _buildingType.ToString();
    }

    private void OnDestroy()
    {
        _radioBtn.onClick.RemoveAllListeners();
    }
}
