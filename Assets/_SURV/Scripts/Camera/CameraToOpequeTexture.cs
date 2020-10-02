using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToOpequeTexture : MonoBehaviour
{
	RenderTexture renTex;
    // Start is called before the first frame update
    void Start()
    {
		renTex = GetComponent<Camera>().targetTexture;
		Shader.SetGlobalTexture("_CameraOpaqueTexture", renTex);
		Debug.Log("Set");
	}

    // Update is called once per frame
    void Update()
    {
		Graphics.SetRenderTarget(renTex);
		GL.Clear(true, true, Color.clear);
	}
}
