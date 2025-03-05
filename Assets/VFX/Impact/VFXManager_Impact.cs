using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.ProBuilder;

public class VFXManager_Impact : MonoBehaviour
{
    private float StartTime;
    void Start() {
        StartTime = Time.realtimeSinceStartup;
    }
    void Update() {
        if (Mathf.Abs(Time.realtimeSinceStartup - StartTime) > 0.5f) Destroy(gameObject);
    }
}
