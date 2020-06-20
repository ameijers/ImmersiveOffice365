using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandTracking : MonoBehaviour, IMixedRealitySourceStateHandler 
{
    public void OnSourceDetected(SourceStateEventData eventData)
    {
        IMixedRealityHand hand = eventData.Controller as IMixedRealityHand;

        if (hand != null)
        {
            if (hand.TryGetJoint(TrackedHandJoint.Wrist, out MixedRealityPose pose))
            {
                Vector3 positionOfWrist = pose.Position;
            }
        }
    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
    }

    public void Update()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Any, out MixedRealityPose pose))
        {
            Vector3 positionOfWrist = pose.Position;
            IMixedRealityHand whichHand = HandJointUtils.FindHand(Handedness.Any);
        }
    }
}
