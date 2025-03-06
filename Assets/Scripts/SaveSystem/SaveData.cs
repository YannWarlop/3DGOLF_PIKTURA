using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SaveData
{
    public bool LV1;
    public bool LV2;
    public bool LV3;
    public bool LV4;
    public bool LV5;

    public string currentScene; //CurrentSceneName

    public SaveData(PlayerData playerData) {// On herite des infos de playerData
        LV1 = playerData.LV1;
        LV2 = playerData.LV2;
        LV3 = playerData.LV3;
        LV4 = playerData.LV4;
        LV5 = playerData.LV5;
        currentScene = playerData.currentScene;
    }
}
