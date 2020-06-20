using EpForceDirectedGraph.cs;
using Office365DataHub.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInformation : MonoBehaviour
{
    public Node node = null;
    public BaseEntity entity = null;

    private TextMesh textMesh = null;

    private void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMesh>();
    }

    private void Update()
    {
        if (textMesh != null && node != null)
        {
            PersonEntity person = entity as PersonEntity;
            if (person != null)
            {
                textMesh.text = person.FullName;
            }
        }
    }
}
