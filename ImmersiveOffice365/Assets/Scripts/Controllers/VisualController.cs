using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController : MonoBehaviour
{
    public void ToggleActive()
    {
        gameObject.SetActive(!isActiveAndEnabled);
    }
}
