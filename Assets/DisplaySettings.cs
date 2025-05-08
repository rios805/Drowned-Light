using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplaySettings : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle; 

    private Resolution[] resolutions;

    void Start()
    {
        // Setup quality
        int savedQualityLevel = PlayerPrefs.GetInt("qualityLevel", QualitySettings.GetQualityLevel());
        QualitySettings.SetQualityLevel(savedQualityLevel);

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string> { "High", "Medium", "Low" });

        int qualityIndex = QualityLevelToDropdownIndex(savedQualityLevel);
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();
        qualityDropdown.onValueChanged.AddListener((index) => SetQuality(index));

        // Setup resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (!options.Contains(option)) // Avoid duplicates
                options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        int savedResolutionIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = Mathf.Clamp(savedResolutionIndex, 0, options.Count - 1);
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = Screen.fullScreen;
    }

    void SetQuality(int dropdownIndex)
    {
        int unityIndex = DropdownIndexToQualityLevel(dropdownIndex);
        unityIndex = Mathf.Clamp(unityIndex, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(unityIndex, true);

        PlayerPrefs.SetInt("qualityLevel", unityIndex);
        PlayerPrefs.Save();
    }

    void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

        PlayerPrefs.SetInt("resolutionIndex", index);
        PlayerPrefs.Save();
    }

    int DropdownIndexToQualityLevel(int index)
    {
        int max = QualitySettings.names.Length - 1;
        switch (index)
        {
            case 0: return max;        // High
            case 1: return max / 2;    // Medium
            case 2: return 0;          // Low
            default: return max / 2;
        }
    }

    int QualityLevelToDropdownIndex(int qualityLevel)
    {
        int max = QualitySettings.names.Length - 1;
        if (qualityLevel >= max) return 0;  // High
        if (qualityLevel <= 0) return 2;    // Low
        return 1;                           // Medium
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("fullscreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
