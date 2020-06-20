using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    public GameObject Root = null;

    public GameObject DraggableContainerPrefab = null;

    public GameObject ContainerPrefab = null;

    public GameObject PersonPrefab = null;

    public string ContainerTitle = "";

    public void CreateContainer()
    {
        // create draggablecontainer
        GameObject draggable = GameObject.Instantiate(DraggableContainerPrefab);
        draggable.transform.parent = Root.transform;

        // create container
        GameObject container = GameObject.Instantiate(ContainerPrefab);
        container.transform.parent = draggable.transform;
        
        // set title
        Transform title = draggable.transform.Find("Header/Title");
        title.GetComponent<TextMeshPro>().text = ContainerTitle;

        // create dummy persons
        for (int i = 0; i < 9; i++)
        {
            GameObject person = GameObject.Instantiate(PersonPrefab);
            person.transform.parent = container.transform;
        }

        container.GetComponent<GridObjectCollection>().UpdateCollection();

        // set position and rotation according to camera
        draggable.transform.position = Camera.main.transform.position;
        draggable.transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        draggable.transform.Translate(Vector3.forward * 2, Space.Self);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateContainer();
        }
    }
}
