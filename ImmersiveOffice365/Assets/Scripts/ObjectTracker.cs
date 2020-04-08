using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
{
    void Update()
    {
        Vector3 gazeDirection = Vector3.zero;
        GameObject target = null;
        Vector3 origin = Vector3.zero;
        Vector3 pointer = Vector3.zero;

        if (CoreServices.InputSystem.GazeProvider.GazeTarget != null)
        {
            gazeDirection = CoreServices.InputSystem.GazeProvider.GazeDirection;
            target = CoreServices.InputSystem.GazeProvider.GazeTarget;
            origin = CoreServices.InputSystem.GazeProvider.GazeOrigin;
            pointer = CoreServices.InputSystem.GazeProvider.GazePointer.Position;
        }
    }
}
