using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireframe : MonoBehaviour
{

    private Material lineMaterial;
    public Color mainColor = new Color(0f, 1f, 0f, 1f);
    private Dictionary<int, int[]> accounts = new Dictionary<int, int[]>();

    void CreateLineMaterial()
    {

        if (!lineMaterial)
        {
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                "SubShader { Pass { " +
                "    Blend SrcAlpha OneMinusSrcAlpha " +
                "    ZWrite Off Cull Off Fog { Mode Off } " +
                "    BindChannels {" +
                "      Bind \"vertex\", vertex Bind \"color\", color }" +
                "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnPostRender()
    {

        //Creates material if it doesn't exist yet.
        CreateLineMaterial();
        
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);
        GL.Color(mainColor);


    }
}
