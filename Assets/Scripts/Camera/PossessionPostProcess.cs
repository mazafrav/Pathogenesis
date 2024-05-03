using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionPostProcess : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField]
    private Shader shader;

    private Material material;

    [SerializeField]
    private float vignetteRadius = 0.0f;
    [SerializeField]
    private Color vignetteColor = Color.black;
    // Start is called before the first frame update
    void Start()
    {
        material = new Material(shader);
        material.SetFloat("_Radius", vignetteRadius);
        material.SetColor("_VignetteColor", vignetteColor);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //if (!isActive)
        //{
        //    Graphics.Blit(source, destination);
        //}
        //else
        //{
        //    Graphics.Blit(source, destination, material);
        //}

        Graphics.Blit(source, destination, material);
    }
}
