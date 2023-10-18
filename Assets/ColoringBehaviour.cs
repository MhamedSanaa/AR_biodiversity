using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringBehaviour : MonoBehaviour
{
    public Camera paintingCamera;
    public GameObject objectToPaint;
    public Texture2D brushTexture;
    public Color paintColor = Color.red;
    public float brushSize = 0.1f;

    private RaycastHit hit;
    private Renderer objectRenderer;
    private Texture2D canvasTexture;
    private bool isPainting = false;

    void Start()
    {
        objectRenderer = objectToPaint.GetComponent<Renderer>();
        if (objectRenderer)
        {
            canvasTexture = new Texture2D(1024, 1024);
            objectRenderer.material.mainTexture = canvasTexture;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isPainting = true;
            Ray ray = paintingCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Paint();
            }
        }
        else
        {
            isPainting = false;
        }
    }

    void Paint()
    {
        if (isPainting && objectRenderer != null)
        {
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= canvasTexture.width;
            pixelUV.y *= canvasTexture.height;

            int posX = (int)pixelUV.x;
            int posY = (int)pixelUV.y;

            for (int x = posX - (int)(brushSize * canvasTexture.width); x < posX + (int)(brushSize * canvasTexture.width); x++)
            {
                for (int y = posY - (int)(brushSize * canvasTexture.height); y < posY + (int)(brushSize * canvasTexture.height); y++)
                {
                    if (x >= 0 && x < canvasTexture.width && y >= 0 && y < canvasTexture.height)
                    {
                        canvasTexture.SetPixel(x, y, paintColor);
                    }
                }
            }
            canvasTexture.Apply();
        }
    }
}