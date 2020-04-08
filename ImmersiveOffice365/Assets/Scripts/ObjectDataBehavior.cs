using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class ObjectDataBehavior : MonoBehaviour, IMixedRealityPointerHandler
{
    public float DistanceToAttach = 0.1f; // 10 centimeter

    private Transform initialCollection = null;

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        // store the collection
        initialCollection = gameObject.transform.parent;

        // detach from the collection
        gameObject.transform.parent = null;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        // follow the position and rotation of the pointer during dragging
        gameObject.transform.position = eventData.Pointer.Position;
        gameObject.transform.rotation = eventData.Pointer.Rotation;
    } 

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        // determine if there is a nearby collection
        GridObjectCollection[] objects = initialCollection.parent.GetComponentsInChildren<GridObjectCollection>();

        float distance = 1f;
        GameObject closedObject = null;
        
        foreach(GridObjectCollection obj in objects)
        {
            GameObject go = obj.gameObject; 

            float dist = Vector3.Distance(go.transform.position, gameObject.transform.position);
            if (dist < distance)
            {
                distance = dist;
                closedObject = go;
            }
        }

        // update the parent to the closest collection 
        if (closedObject != null)
        {
            gameObject.transform.parent = closedObject.transform;
        }
        else
        {
            gameObject.transform.parent = initialCollection;
        }

        // update the collection it is attached to
        GridObjectCollection goc = gameObject.transform.parent.GetComponent<GridObjectCollection>();
        if (goc != null)
        {
            goc.UpdateCollection();
        }
    }

    public void ObjectMoved()
    {

    }
}
