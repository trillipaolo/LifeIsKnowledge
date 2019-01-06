using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; //to modify a UI element

public class OptionMenuManager : MonoBehaviour {

    public AudioMixer masterMixer;

    public Dropdown resolutionDropdown; //get the dropdown component
    Resolution[] resolutions;

    void Start() {
        resolutions = Screen.resolutions; //resolutions at disposal at startup: is an array
        resolutionDropdown.ClearOptions(); //at startup clear the list

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i =0; i< resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); //it accepts only lists so we convert the array into a list (see the for above)
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height, Screen.fullScreen);
    }

    public void SetMusicLvl (float MusicLvl) {
        masterMixer.SetFloat("musicVol", MusicLvl);
    }

    public void SetSfxLvl(float SfxLvl)
    {
        masterMixer.SetFloat("sfxVol", SfxLvl);
    }

    public void SetQuality (int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }
}
