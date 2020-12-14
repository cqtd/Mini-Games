using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraHandler : MonoBehaviour
{
    private void OnEnable()
    {
        UniversalAdditionalCameraData cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();
        cameraData.cameraStack.AddRange(FindObjectsOfType<Camera>()
            .Where(e => e.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay));
    }

    private void OnDisable()
    {
        
    }
}
