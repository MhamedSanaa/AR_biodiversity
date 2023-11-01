using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehaviour : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private float movementSpeed = 1f;
    private float movementRange = 0.5f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Rotation around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Vertical movement
        Vector3 newPosition = initialPosition + new Vector3(0f, Mathf.Sin(Time.time * movementSpeed) * movementRange, 0f);
        transform.position = newPosition;
    }
}
