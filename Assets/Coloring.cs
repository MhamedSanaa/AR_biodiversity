using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Coloring : MonoBehaviour
{
    public Camera mainCamera;
    public Texture2D brushTexture;
    public float brushSize = 1f;
    public Color brushColor = Color.red;

    private RaycastHit hitInfo;
    private Renderer rend;
    private Texture2D canvasTexture;
    private Vector2 storedUV;

    void Start()
    {
        rend = GetComponent<Renderer>();
        canvasTexture = new Texture2D(1024, 1024);
        canvasTexture.wrapMode = TextureWrapMode.Clamp;
        rend.material.mainTexture = canvasTexture;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                Debug.Log(hitInfo.transform.name);
                if (hitInfo.transform == transform)
                {
                    Debug.Log(hitInfo.transform.name);
                    Debug.Log("cccccccccccccccccccccccccccc",transform);
                    Debug.Log(hitInfo.textureCoord);
                    Paint(hitInfo.textureCoord);
                }
            }
        }
    }

    void Paint(Vector2 uv)
    {
        int x = (int)(uv.x * canvasTexture.width);
        int y = (int)(uv.y * canvasTexture.height);

        for (int i = -Mathf.RoundToInt(brushSize); i < Mathf.RoundToInt(brushSize); i++)
        {
            for (int j = -Mathf.RoundToInt(brushSize); j < Mathf.RoundToInt(brushSize); j++)
            {
                if (x + i >= 0 && x + i < canvasTexture.width && y + j >= 0 && y + j < canvasTexture.height)
                {
                    canvasTexture.SetPixel(x + i, y + j, brushColor);
                }
            }
        }
        canvasTexture.Apply();
    }
}