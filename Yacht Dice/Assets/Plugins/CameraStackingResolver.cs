using System.Linq;
using UnityEngine;
// using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class CameraStackingResolver : MonoBehaviour
{
    // private UniversalAdditionalCameraData cameraData = default;
    //
    // private void OnEnable()
    // {
    //     cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();
    //     cameraData.cameraStack.AddRange(FindObjectsOfType<Camera>()
    //         .Where(e => e.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay));
    // }
    //
    // private void OnDisable()
    // {
    //     cameraData.cameraStack.Clear();
    // }
}
