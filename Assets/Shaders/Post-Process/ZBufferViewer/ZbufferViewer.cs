using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZbufferViewer : MonoBehaviour
{
    public Shader ZbufferViewerShader;
    private Material ZbufferViewerMaterial;
    private void Start(){
        ZbufferViewerMaterial = new Material(ZbufferViewerShader);
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }
    
    void OnRenderImage(RenderTexture source, RenderTexture destination){
        Graphics.Blit(source, destination, ZbufferViewerMaterial);
    }
}
