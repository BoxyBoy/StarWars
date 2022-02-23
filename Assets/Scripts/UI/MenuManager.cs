using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public Toggle fullScreenToggle;
    public int[] screenWidths;

    float aspectRatio = 16 / 9f;
    int activeScreenResolutionIndex;

    private void Start()
    {
        // Load Player Preferences
        activeScreenResolutionIndex = PlayerPrefs.GetInt("ActiveScreenResolutionIndex", activeScreenResolutionIndex);
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 0) == 1 ? true : false;

        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[1].value = AudioManager.instance.sfxVolumePercent;
        volumeSliders[2].value = AudioManager.instance.musicVolumePercent;

        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = (i == activeScreenResolutionIndex);
        }

        fullScreenToggle.isOn = isFullscreen;
    }

    public void StartCampaign()
    {
        //SceneManager.LoadScene("Kamino")
    }

    public void Play()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu()
    {
        optionsMenuHolder.SetActive(false);
        mainMenuHolder.SetActive(true);
    }

    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResolutionIndex = i;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);

            // Save Player Preferences
            PlayerPrefs.SetInt("ActiveScreenResolutionIndex", activeScreenResolutionIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;
        }

        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, isFullscreen);
        }
        else
        {
            SetScreenResolution(activeScreenResolutionIndex);
        }

        // Save Player Preferences
        PlayerPrefs.SetInt("Fullscreen", (isFullscreen ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.MASTER);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.SFX);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.MUSIC);
    }
}
