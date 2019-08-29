using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class CHDButton : MonoBehaviour, IInputClickHandler
{
    public CameraHeightAdjustment cameraHeightAdjustment;

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        cameraHeightAdjustment.decreaseCameraHeight();
        //Debug.Log("CHD Button clicked!");
    }
}
