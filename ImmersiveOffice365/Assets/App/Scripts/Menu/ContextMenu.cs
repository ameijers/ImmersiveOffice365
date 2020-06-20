using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    public GameObject ContextMenuPrefab = null;

    public float ActivationDistance = 0.4f;

    private GameObject contextMenu = null;
    private AppBar contextMenuBar = null;

    // Start is called before the first frame update
    void Start()
    {
        contextMenu = GameObject.Instantiate(ContextMenuPrefab);
        contextMenuBar = contextMenu.GetComponent<AppBar>();
        contextMenu.SetActive(false);

        // add listener to remove button
        Transform buttonRemove = contextMenu.transform.Find("BaseRenderer/ButtonParent/Remove");
        PressableButtonHoloLens2 button = buttonRemove.GetComponent<PressableButtonHoloLens2>();
        button.ButtonPressed.AddListener(Remove);
    }

    // Update is called once per frame
    void Update()
    {
        MixedRealityPose pose;

        // determine if any of the index tip fingers are tracked
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out pose))
        {
            BoundingBox[] collection = gameObject.GetComponentsInChildren<BoundingBox>();

            // find closest collection
            float distance = ActivationDistance;

            BoundingBox activeBox = null;

            foreach (BoundingBox item in collection)
            {
                float dist = Vector3.Distance(item.gameObject.transform.position, pose.Position);

                if (dist < distance)
                {
                    distance = dist;
                    activeBox = item;
                }
            }

            if (activeBox != null)
            {
                contextMenu.SetActive(true);
                contextMenuBar.Target = activeBox;             
            }
            else
            {
                contextMenu.SetActive(false);
            }
        }
        else
        {
            contextMenu.SetActive(false);
            gameObject.GetComponent<GridObjectCollection>().UpdateCollection();
        }
    }

    public void Remove()
    {
        GameObject.Destroy(contextMenuBar.Target.gameObject);
    }
}
