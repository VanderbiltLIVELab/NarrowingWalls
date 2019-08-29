using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SWDButton : MonoBehaviour, IInputClickHandler
{
    public ShoulderWidthAdjustment shoulderWidthAdjustment;

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        shoulderWidthAdjustment.decreaseShoulderWidth();
        //Debug.Log("SWD Button clicked!");
    }
}
