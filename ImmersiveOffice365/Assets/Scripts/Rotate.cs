using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 100f;

    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * Speed * Time.deltaTime);
    }
}
