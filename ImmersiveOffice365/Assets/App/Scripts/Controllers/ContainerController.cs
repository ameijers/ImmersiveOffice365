using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    public GameObject Root = null;

    public GameObject DraggableContainerPrefab = null;

    public GameObject ContainerPrefab = null;

    public string ContainerTitle = "";

    public void CreateContainer()
    {
        // create draggablecontainer
        GameObject draggable = GameObject.Instantiate(DraggableContainerPrefab);
        draggable.transform.parent = Root.transform;

        // create container
        GameObject container = GameObject.Instantiate(ContainerPrefab);
        container.transform.parent = draggable.transform;

        Transform title = draggable.transform.Find("Header/Title");
        title.GetComponent<TextMeshPro>().text = ContainerTitle;

    }
}
