using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible2D : MonoBehaviour
{

    public float BobbingSpeed = 4f; // Speed of bobbing
    public float BobbingHeight = 0.1f; // Height of bobbing
    public GameObject onCollectEffect;

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Bob up and down with a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * BobbingSpeed) * BobbingHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}


