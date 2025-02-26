using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WinLoseCondition winLoseCondition;
    [SerializeField] private string _nextSceneName;
    void Update() {
        if (winLoseCondition != null)
        {
            if (winLoseCondition.WinLoseFlag == "Win")SceneManager.LoadScene(_nextSceneName);
            else if (winLoseCondition.WinLoseFlag == "Lose")SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void StartGame() {
        SceneManager.LoadScene(_nextSceneName); 
    }
    
}
