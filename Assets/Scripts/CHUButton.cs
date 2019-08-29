using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class CHUButton : MonoBehaviour, IInputClickHandler
{
    public CameraHeightAdjustment cameraHeightAdjustment;

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        cameraHeightAdjustment.increaseCameraHeight();
        //Debug.Log("CHU Button clicked!");
    }
}
