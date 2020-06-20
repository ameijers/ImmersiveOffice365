using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAwareness : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Use CoreServices to quickly get access to the IMixedRealitySpatialAwarenessSystem
        var spatialAwarenessService = CoreServices.SpatialAwarenessSystem;

        // Cast to the IMixedRealityDataProviderAccess to get access to the data providers
        var dataProviderAccess = spatialAwarenessService as IMixedRealityDataProviderAccess;

        var meshObserver = dataProviderAccess.GetDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();

        meshObserver.Disable();
    }
}
