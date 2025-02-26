using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcssOutline : MonoBehaviour {
    public Shader outlineShader;
    
    private Material outlineMaterial;
    void OnEnable() {
        outlineMaterial = new Material(outlineShader);
        outlineMaterial.hideFlags = HideFlags.HideAndDontSave;
    }
    void OnDisable() {
        outlineMaterial = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, outlineMaterial, 1);
    }
}