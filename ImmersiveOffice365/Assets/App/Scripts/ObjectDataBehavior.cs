using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro;
using UnityEngine;

public class ObjectDataBehavior : MonoBehaviour, IMixedRealityPointerHandler
{
    public float DistanceToAttach = 0.1f; // 10 centimeter

    private Transform initialCollection = null;

    private bool isPartOfGraphController = false;

    private GameObject objectToMove = null;

    private GameObject Duplicate()
    {
        return GameObject.Instantiate(gameObject);
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        objectToMove = gameObject;

        // store the collection
        initialCollection = objectToMove.transform.parent;

        // check if part of the model
        isPartOfGraphController = initialCollection.GetComponent<GraphController>() != null ? true : false;

        if (isPartOfGraphController)
        {
            objectToMove = Duplicate();
        }

        // detach from the collection
        objectToMove.transform.parent = null;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        if (objectToMove != null)
        {
            // follow the position and rotation of the pointer during dragging
            objectToMove.transform.position = eventData.Pointer.Position;
            objectToMove.transform.rotation = eventData.Pointer.Rotation;
        }
    } 

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        // get all the grid collections created in the scene
        // parent = DraggableContainer or DraggableModel
        // parent.parent = Containers
        GridObjectCollection[] collections = initialCollection.parent.parent.GetComponentsInChildren<GridObjectCollection>();

        // find closest collection
        float distance = 1f;
        GameObject closestCollection = null;
        
        foreach(GridObjectCollection collection in collections)
        {
            GameObject go = collection.gameObject; 

            float dist = Vector3.Distance(go.transform.position, objectToMove.transform.position);
            if (dist < distance)
            {
                distance = dist;
                closestCollection = go;
            }
        }

        // update the parent to the closest collection 
        if (closestCollection != null)
        {
            // set the closest collection as parent
            objectToMove.transform.parent = closestCollection.transform;

            // update the closest collection
            GridObjectCollection goc = closestCollection.GetComponent<GridObjectCollection>();
            goc.UpdateCollection();

            if (!isPartOfGraphController)
            {
                // update the initial collection when no model
                GridObjectCollection gocInitial = initialCollection.GetComponent<GridObjectCollection>();
                gocInitial.UpdateCollection();
            }

        }
        else
        {
            if (isPartOfGraphController)
            {
                // remove duplicated item since no closest collection was found
                GameObject.Destroy(objectToMove);
            }
            else
            {
                // set back to the initial collection
                objectToMove.transform.parent = initialCollection;

                // update the initial collection
                GridObjectCollection gocInitial = initialCollection.GetComponent<GridObjectCollection>();
                gocInitial.UpdateCollection();
            }
        }
    }

    public void ObjectMoved()
    {

    }
}
