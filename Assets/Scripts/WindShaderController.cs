using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindShaderController : MonoBehaviour
{
    // All of the custom wind shader materials
    public Material[] materials;

    // Global wind speed scalar
    public float globalWindSpeed;

    void Start()
    {
        foreach (Material material in materials)
        {
            material.SetFloat("_WindSpeed", globalWindSpeed);
        }
    }
}
