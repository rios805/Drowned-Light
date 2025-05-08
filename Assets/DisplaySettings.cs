using UnityEngine;
using TMPro;

public class DisplaySettings : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;

    void Start()
    {
        // Load saved quality level or use the current
        int savedQualityLevel = PlayerPrefs.GetInt("qualityLevel", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQualityLevel);

        // Set up dropdown options
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string> { "High", "Medium", "Low" });

        // Match dropdown value to current quality
        int dropdownIndex = QualityLevelToDropdownIndex(savedQualityLevel);
        qualityDropdown.value = dropdownIndex;
        qualityDropdown.RefreshShownValue();

        // Add listener for changes
        qualityDropdown.onValueChanged.AddListener((index) => {
            SetQuality(index);
        });

        // Debug quality levels
        Debug.Log("Unity Quality Levels:");
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            Debug.Log($"{i}: {QualitySettings.names[i]}");
        }
    }

    void SetQuality(int dropdownIndex)
    {
        int unityIndex = DropdownIndexToQualityLevel(dropdownIndex);
        unityIndex = Mathf.Clamp(unityIndex, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(unityIndex, true);

        PlayerPrefs.SetInt("qualityLevel", unityIndex);
        PlayerPrefs.Save();

        Debug.Log("Quality set to: " + QualitySettings.names[unityIndex]);
    }

    int DropdownIndexToQualityLevel(int index)
    {
      
        int max = QualitySettings.names.Length - 1;

        switch (index)
        {
            case 0: return max;               // High = highest quality index
            case 1: return max / 2;           // Medium = middle quality index
            case 2: return 0;                 // Low = lowest quality index
            default: return max / 2;
        }
    }

    int QualityLevelToDropdownIndex(int qualityLevel)
    {
        int max = QualitySettings.names.Length - 1;

        if (qualityLevel >= max) return 0;   // High
        if (qualityLevel <= 0) return 2;     // Low
        return 1;                            // Medium
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
