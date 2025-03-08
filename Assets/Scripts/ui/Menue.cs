using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menue : MonoBehaviour
{
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
        Application.Quit();
    }
    public void OnClickSettings()
    {
        SceneManager.LoadScene("Settings");
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
    public void OnclickReturn()
    {
        SceneManager.LoadScene("MainMenu");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
