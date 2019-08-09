﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SWUButton : MonoBehaviour, IInputClickHandler
{
    public ShoulderWidthAdjustment shoulderWidthAdjustment;

    private void Start()
    {
        //InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        shoulderWidthAdjustment.increaseShoulderWidth();
        //Debug.Log("SWU Button clicked!");
    }
}