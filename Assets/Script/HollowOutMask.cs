using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HollowOutMask : MonoBehaviour
{
    Material material;

    private void Awake()
    {
        GetMaterial();
    }

    public Material GetMaterial()
    {
        Image image = GetComponent<Image>();

        image.material = new Material(image.material);

        material = image.material;

        return material;
    }


    public void RefreshMaterial(Vector3 center, float width, float height)
    {
        if(material)
        {
            material.SetVector("_Center", new Vector4(center.x, center.y, 0, 0));
            material.SetVector("_Size", new Vector4(width * 0.5f, height * 0.5f, 0, 0));
        }
    }
}