using System.Collections;
using System.Collections.Generic;
using TMPro;
//using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class settingsMenu : MonoBehaviour
{
    public Slider musicSlider; // Slider per al volum de música
    public Slider sfxSlider;
   // public AudioSource music;
   // public AudioSource sfx;
    public TMP_Dropdown res_dropdown;
    Resolution[] resolutions;
   // public GameObject game_manager;
    bool active = false;
    // Start is called before the first frame update
    private void Start()
    {
        SyncSlidersWithVolumeSettings();

        resolutions = Screen.resolutions;
        res_dropdown.ClearOptions();
        int currentResolutionIn = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIn = i;

            }
        }
        res_dropdown.AddOptions(options);
        res_dropdown.value = currentResolutionIn;
        res_dropdown.RefreshShownValue();
        
    }
    public void SetResolution(int resolution_index)
    {
        Resolution resolution = resolutions[resolution_index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolumeMusic(float volume)
    {
        // Guarda el volum i actualitza l'AudioManager
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();

       // AudioManager audioManager = FindObjectOfType<AudioManager>();
       // if (audioManager != null)
      //  {
      //      audioManager.SetMusicVolume(volume);
       // }
    }

    public void SetVolumeSFX(float volume)
    {
        // Guarda el volum i actualitza l'AudioManager
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();

        //AudioManager audioManager = FindObjectOfType<AudioManager>();
       // if (audioManager != null)
       // {
      //      audioManager.SetSFXVolume(volume);
      //  }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void exit()
    {
        Resume();
    }


    public void Pause()
    {
        if (!active)
        {
            //game_manager.SetActive(false);
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            active = true;
        }
        else Resume();
    }
    public void Resume()
    {
        if (active)
        {
           // if (game_manager != null)
           // {
           //     game_manager.SetActive(true);
           // }
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            active = false;
        }
        else Pause();
    }
    //public void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.Escape))
     //   {
         //   Resume();
      //  }
    //}
    public void PauseFromStart()
    {
        gameObject.SetActive(true);

        active = true;
    }

     private void OnEnable()
    {
        // Tornar a sincronitzar cada vegada que el menú de configuració es mostri
        SyncSlidersWithVolumeSettings();
    }

    private void SyncSlidersWithVolumeSettings()
    {
        // Carregar els valors guardats de PlayerPrefs
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Establir els valors als sliders
        if (musicSlider != null)
            musicSlider.value = savedMusicVolume;

        if (sfxSlider != null)
            sfxSlider.value = savedSFXVolume;
    }
}