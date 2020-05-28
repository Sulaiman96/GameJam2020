using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHandler : MonoBehaviour
{
    public Material defaultMaterial;
    public Material activeMaterial;
    public bool isActiveMaterial { get; set; } = false;

    private MeshRenderer meshRender;
    // Start is called before the first frame update
    void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();
        if (defaultMaterial)
        {
            meshRender.material = defaultMaterial;
        }
        else
        {
            // If we don't have default material we store current material
            defaultMaterial = meshRender.material;
        }
    }

    public void UseActiveMaterial()
    {
        if (!activeMaterial)
            return;

        meshRender.material = activeMaterial;
        isActiveMaterial = true;
    }

    public void UseDefaultMaterial()
    {
        if (!defaultMaterial)
            return;

        meshRender.material = defaultMaterial;
        isActiveMaterial = false;
    }

    public void ChangeActiveMaterial(Material mat)
    {
        activeMaterial = mat;
    }
}
