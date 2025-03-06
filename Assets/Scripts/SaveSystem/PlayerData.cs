using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    // Save Bools - Niveau
    public bool LV1;
    public bool LV2;
    public bool LV3;
    public bool LV4;
    public bool LV5;

    public string currentScene; // CurrentScenename

    private void Awake() {
        if (SceneManager.GetActiveScene().name != "MainMenu") {
            LoadPlayer();
        }
    }
    // LOAD / SAVE
    public void SavePlayer() {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer() {
        SaveData data = SaveSystem.LoadPlayer();
        LV1 = data.LV1;
        LV2 = data.LV2;
        LV3 = data.LV3;
        LV4 = data.LV4;
        LV5 = data.LV5;
        currentScene = data.currentScene;
    }
    // EDIT DE VARIABLES
    public void EditBoolsJockey(bool value) {
        LV1 = (value);
    }
    public void EditBoolsMafieux(bool value) {
        LV2 = (value);
    }
    public void EditBoolsMedecin(bool value) {
        LV3 = (value);
    }
    public void EditBoolsBookmaker(bool value) {
        LV4 = (value);
    }
    public void EditBoolsOrganisateur(bool value) {
        LV5 = (value);
    }
    public void EditCurrentScene() {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void NewGame() { // Clear les save files, ecraser la sauvegarde
        LV1 = false;
        LV2 = false;
        LV3 = false;
        LV4 = false;
        LV5 = false;
        SavePlayer();
    }

    public void ContinueGame() { // On charge la dernière sauvegarde 
        LoadPlayer();
    }
}