using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _quitMenu;
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] private GameObject _audioVolumeSlider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);
            Time.timeScale = Convert.ToInt32(!Convert.ToBoolean(Time.timeScale)); //A TESTER
        }
    }

    public void Continue() {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void LevelSelect() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void SettingsMenu() {
        _settingsMenu.SetActive(true);
    }

    public void QuitGame() {
        _quitMenu.SetActive(true);
    }

    public void ConfirmQuitGame()
    {
        Application.Quit();
    }

    public void AdjustVolume()
    {
        
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(_audioVolumeSlider.GetComponent<Slider>().value)*20);
    }
}
