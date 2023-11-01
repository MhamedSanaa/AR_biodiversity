using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoringBehaviour : MonoBehaviour
{
    public Camera mainCamera;
    public float brushSize = 50f;
    public GameObject brushColorObject;
    public Slider SizeSlider;
    private Color brushColor;
    private LineRenderer lineRenderer;
    private Texture2D brushTexture;
    private Texture2D canvasTexture;
    private RaycastHit hitInfo;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        canvasTexture = new Texture2D(1024, 1024);
        rend.material.mainTexture = canvasTexture;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        brushColor = brushColorObject.GetComponent<Image>().color;
        brushSize = SizeSlider.value;
        brushTexture = CreateBrushTexture((int)brushSize, brushColor);
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                //Debug.Log("touched obejct");
                //Debug.Log(hitInfo.transform.name);

                if (hitInfo.transform == transform)
                {
                    Debug.Log("hit obejct-----------------------------");
                    Debug.Log(hitInfo.transform.name);
                    Debug.Log("obejct-----------------------------");
                    Debug.Log(transform.name);
                    Paint(hitInfo.textureCoord);
                }
            }
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
        }
    }

    Texture2D CreateBrushTexture(int size, Color color)
    {
        Texture2D tex = new Texture2D(size, size);
        Color[] colors = new Color[size * size];

        float center = size / 2f;
        float radius = size / 2f;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float distance = Vector2.Distance(new Vector2(i, j), new Vector2(center, center));
                float alpha = Mathf.Clamp01(1 - distance / radius);
                colors[i * size + j] = color * new Color(1, 1, 1, alpha);
            }
        }

        tex.SetPixels(colors);
        tex.Apply();
        return tex;
    }

    void Paint(Vector2 uv)
    {
        int width = canvasTexture.width;
        int height = canvasTexture.height;

        int x = (int)(uv.x * width);
        int y = (int)(uv.y * height);

        int brushWidth = brushTexture.width;
        int brushHeight = brushTexture.height;

        int textureX, textureY;
        for (int i = 0; i < brushWidth; i++)
        {
            for (int j = 0; j < brushHeight; j++)
            {
                textureX = x + i - brushWidth / 2;
                textureY = y + j - brushHeight / 2;

                if (textureX >= 0 && textureX < width && textureY >= 0 && textureY < height)
                {
                    Color canvasColor = canvasTexture.GetPixel(textureX, textureY);
                    Color brushColored = brushTexture.GetPixel(i, j) * brushColor;
                    float dist = Vector2.Distance(new Vector2(i, j), new Vector2(brushWidth / 2, brushHeight / 2));
                    float strength = 1 - Mathf.Clamp01(dist / (brushWidth / 2));
                    Color finalColor = Color.Lerp(canvasColor, brushColored, brushColored.a * strength);
                    canvasTexture.SetPixel(textureX, textureY, finalColor);
                }
            }
        }
        canvasTexture.Apply();
    }
}