using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObjectsX : MonoBehaviour
{
    public float spinSpeed;
    float y;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        }
        else if (gameObject.CompareTag("Bomb") || gameObject.CompareTag("Money"))
        {
            transform.RotateAround(Vector3.zero, Vector3.up, 2*Time.deltaTime);
        }
    }
}
