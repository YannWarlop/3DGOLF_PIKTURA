using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.ProBuilder;

public class VFXManager_Impact : MonoBehaviour
{
    private float startTime;
    private Material VFXMaterial;
    void Start() {
        startTime = Time.time;
        VFXMaterial = GetComponent<MeshRenderer>().material;
        VFXMaterial.SetFloat("_StartTime", Time.time );
        
        Debug.Log(gameObject.transform.localScale);
    }
    void Update() {
        if (Mathf.Abs(Time.time - startTime) > 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
