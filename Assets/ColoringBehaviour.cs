using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoringBehaviour : MonoBehaviour
{
    public Camera mainCamera;
    public Texture2D brushTexture;
    public float brushSize = 1f;
    public Color brushColor;
    public GameObject brushColorObject;
    private LineRenderer lineRenderer;
    private Renderer rend;
    private Texture2D canvasTexture;
    void Start()
    {
        Application.targetFrameRate = 10;
        rend = GetComponent<Renderer>();
        canvasTexture = new Texture2D(1024, 1024);
        canvasTexture.wrapMode = TextureWrapMode.Clamp;
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
        //Debug.Log("IsMouseInput");
        //Debug.Log(Input.GetMouseButton(0));
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
       /* Debug.Log("IsTouchInput");
        Debug.Log(Input.GetTouch(0).phase == TouchPhase.Began);
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); // Cast a ray from the touch position

            RaycastHit hitInfo; // Variable to hold the raycast hit information

            if (Physics.Raycast(ray, out hitInfo)) // Check if the ray hits any collider
            {
                if (hitInfo.transform == transform)
                {
                    Paint(hitInfo.textureCoord);
                }
            }
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
        }*/
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