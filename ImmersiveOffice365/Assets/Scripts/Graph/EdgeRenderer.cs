using EpForceDirectedGraph.cs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeRenderer : MonoBehaviour
{
    public GameObject Source = null;
    public GameObject Target = null;

    private LineRenderer line = null;

    private void Initialize()
    {
        line = gameObject.GetComponent<LineRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Source != null && Target != null & line != null)
        {
            line.SetPositions(new Vector3[] { Source.transform.position, Target.transform.position });
        }

    }
}


