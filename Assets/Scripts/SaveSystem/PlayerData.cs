using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    // organisation des bools
    public List<bool> jockey;
    public List<bool> mafieux;
    public List<bool> medecin;
    public List<bool> bookmaker;
    public List<bool> organisateur;

    public string currentScene; // Nom de la Scene actuelle

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            LoadPlayer();
        }

    }

    // LOAD / SAVE
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        SaveData data = SaveSystem.LoadPlayer();

        jockey = data.jockey;
        mafieux = data.mafieux;
        medecin = data.medecin;
        bookmaker = data.bookmaker;
        organisateur = data.organisateur;

        currentScene = data.currentScene;
    }

    // EDIT DE VARIABLES
    public void EditBoolsJockey(bool value)
    {
        jockey.Add(value);
    }
    public void EditBoolsMafieux(bool value)
    {
        mafieux.Add(value);
    }
    public void EditBoolsMedecin(bool value)
    {
        medecin.Add(value);
    }
    public void EditBoolsBookmaker(bool value)
    {
        bookmaker.Add(value);
    }

    public void EditBoolsOrganisateur(bool value)
    {
        organisateur.Add(value);
    }
    


    public void EditCurrentScene()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void NewGame() // Clear les save files, ecraser la sauvegarde
    {
        jockey.Clear();
        mafieux.Clear();
        medecin.Clear();
        bookmaker.Clear();
        organisateur.Clear();
        
        SavePlayer();
    }

    public void ContinueGame() // On charge la dernière sauvegarde 
    {
        LoadPlayer();
    }
}