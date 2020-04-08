using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    private TextMesh textMesh = null;

    public string DebugText { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMesh>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (textMesh != null)
        {
            textMesh.text = DebugInformation.Instance.LogText;
        }
    }
}
