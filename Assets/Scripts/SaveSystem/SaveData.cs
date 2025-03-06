using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<bool> jockey;
    public List<bool> mafieux;
    public List<bool> medecin;
    public List<bool> bookmaker;
    public List<bool> organisateur;

    public string currentScene; // Nom de la Scene actuelle

    public SaveData(PlayerData playerData) // On herite des infos de playerData
    {
        jockey = playerData.jockey;
        mafieux = playerData.mafieux;
        medecin = playerData.medecin;
        bookmaker = playerData.bookmaker;
        organisateur = playerData.organisateur;
        
        currentScene = playerData.currentScene;
    }
}
