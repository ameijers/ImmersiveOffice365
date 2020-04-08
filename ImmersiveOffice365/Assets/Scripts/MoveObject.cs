using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float distancePerSecond = 0.1f;
    void Update()
    {
        gameObject.transform.Translate(0, 0, distancePerSecond * Time.deltaTime);
    }
}
