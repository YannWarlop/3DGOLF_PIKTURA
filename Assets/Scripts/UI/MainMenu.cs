using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
     [SerializeField] private GameObject _quitMenu;
     [SerializeField] private GameObject _settingsMenu;
     [SerializeField] private GameObject _tutorialMenu;
     
    public void OnClickPlay()
    {
        SceneManager.LoadScene("LV1");
    }
    public void OnClickContinue()
    {
        SceneManager.LoadScene("LevelSelection");
    }
    public void OnclickQuit()
    {
        _quitMenu.SetActive(true);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }
    public void OnClickSettings()
    {
        _settingsMenu.SetActive(true);
    }
    public void OnclickCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void OnclickLv1()
    {
        SceneManager.LoadScene("LV1");
    }
    public void OnclickLv2()
    {
        SceneManager.LoadScene("LV2");
    }
    public void OnclickLv3()
    {
        SceneManager.LoadScene("LV3");
    }
    public void OnclickLv4()
    {
        SceneManager.LoadScene("LV4");
    }
    public void OnclickLv5()
    {
        SceneManager.LoadScene("LV5");
    }
    public void OnclickReturn()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenTutorial()
    {
        _tutorialMenu.SetActive(true);
    }
}
