using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour {

    public static ExperimentManager experimentManager;

    void Start()
    {
        experimentManager = this;
    }

    private bool isPerception = true;

    public Transform camera;
    public Transform primaryUI;
    public Transform secondaryUI;
    public Transform gapWidthIndicatorUI;
    public Transform shoulderWidthAdjustmentUI;
    public Transform cameraHeightAdjustmentUI;

    private int trialNum = 1;
    public bool collided = false;
    private float minGapWidth = 1.5f;

    public void setPrimary (bool input)
    {
        isPerception = input;

        if (isPerception)
        {
            primaryUI.GetComponent<TextMesh>().text = "Perception";
            setIsAscending(true);
            secondaryUI.GetComponent<TextMesh>().text = "Ascending";
        }
        else
        {
            primaryUI.GetComponent<TextMesh>().text = "Practice";
            secondaryUI.GetComponent<TextMesh>().text = "Enter Shoulder Width...";
        }
    }

    //start of perception mode functions
    public void setIsAscending (bool isAscending)
    {
        if (isPerception)
        {
            if (isAscending)
            {
                secondaryUI.GetComponent<TextMesh>().text = "Ascending";
                transform.position = new Vector3(7.8f, transform.position.y, transform.position.z);
            }
            else
            {
                secondaryUI.GetComponent<TextMesh>().text = "Descending";
                transform.position = new Vector3(8.2f, transform.position.y, transform.position.z);
            }
        }
    }

    public void wider ()
    {
        if (isPerception)
            transform.position = new Vector3(0.015f + transform.position.x, transform.position.y, transform.position.z);
    }

    public void narrower ()
    {
        if (isPerception)
        {
            if (transform.position.x - 7.5f >= 0.02f)
                transform.position = new Vector3(-0.02f + transform.position.x, transform.position.y, transform.position.z);
        }
    }

    public void finish ()
    {
        float gapWidth = transform.position.x - 7.5f;
        gapWidthIndicatorUI.GetComponent<TextMesh>().text = gapWidth.ToString("0.000");
    }
    //end of perception mode functions

    //start of practice mode functions
    public void next ()
    {
        if (!isPerception)
        {
            if (trialNum == 1) //subject has set up shoulder width
            {
                gapWidthIndicatorUI.gameObject.SetActive(false);
                shoulderWidthAdjustmentUI.gameObject.SetActive(false);
                cameraHeightAdjustmentUI.gameObject.SetActive(false);
                float sWidth = camera.GetComponent<CapsuleCollider>().height;
                transform.position = new Vector3(sWidth * 1.25f + 7.5f, transform.position.y, transform.position.z); //set gap width to be 25% wider than shoulder width
            }
            else if (trialNum >= 2 && trialNum <= 20)
            {
                if (collided)
                {
                    transform.position = new Vector3(0.015f + transform.position.x, transform.position.y, transform.position.z);
                    collided = false;
                }
                else
                {
                    transform.position = new Vector3(-0.02f + transform.position.x, transform.position.y, transform.position.z);
                }
                secondaryUI.GetComponent<TextMesh>().text = "Trial #" + trialNum.ToString();
            }
            else
            {
                trialNum = 0;
                //log gap width
                gapWidthIndicatorUI.gameObject.SetActive(true);
                shoulderWidthAdjustmentUI.gameObject.SetActive(true);
                cameraHeightAdjustmentUI.gameObject.SetActive(true);
                gapWidthIndicatorUI.GetComponent<TextMesh>().text = minGapWidth.ToString("0.000");
                //reset UI
                secondaryUI.GetComponent<TextMesh>().text = "Finished/Reset. Enter Shoulder Width...";
                minGapWidth = 1.5f;
                collided = false;
            }
            ++trialNum;
            float curGapWidth = transform.position.x - 7.5f;
            if (minGapWidth - curGapWidth > 0.001f) minGapWidth = curGapWidth;
        }
    }
    //end of practice mode functions

    public void resetExperiment ()
    {
        setPrimary(isPerception);
        if (!isPerception)
        {
            trialNum = -1;
            next();
        }
        gapWidthIndicatorUI.GetComponent<TextMesh>().text = " N/A";
    }
}
