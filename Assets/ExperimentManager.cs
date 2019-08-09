using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour {

    public static ExperimentManager experimentManager;
    public Transform camera;

    void Start ()
    {
        experimentManager = this;
    }

    private bool isPerception = true;

    public Transform primaryUI;
    public Transform secondaryUI;
    public Transform wallWidthIndicatorUI;

    public void setPrimary (bool input)
    {
        isPerception = input;

        if (isPerception)
        {
            primaryUI.GetComponent<TextMesh>().text = "Perception";
            secondaryUI.GetComponent<TextMesh>().text = "Ascending";
        }
        else
        {
            primaryUI.GetComponent<TextMesh>().text = "Practice";
            secondaryUI.GetComponent<TextMesh>().text = "Enter Shoulder Width...";
            wallWidthIndicatorUI.gameObject.SetActive(false);
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
                secondaryUI.GetComponent<TextMesh>().text = "Decending";
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

    public void finish()
    {
        float wallWidth = transform.position.x - 7.5f;
        wallWidthIndicatorUI.GetComponent<TextMesh>().text = wallWidth.ToString("0.000");
    }
    //end of perception mode functions

    //start of practice mode functions
    private int trialNum = 0;
    public bool collided = false;
    private float minWallWidth = 1.5f;

    public void next ()
    {
        if (!isPerception)
        {
            if (trialNum == 0) //subject has set up shoulder width
            {
                wallWidthIndicatorUI.gameObject.SetActive(false);
                float sWidth = camera.GetComponent<CapsuleCollider>().height;
                transform.position = new Vector3(sWidth * 1.25f + 7.5f, transform.position.y, transform.position.z); //set wall width to be 25% wider than shoulder width
            }
            else if (trialNum >= 1 && trialNum <= 20)
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
                trialNum = -1;
                //log wall width
                wallWidthIndicatorUI.gameObject.SetActive(true);
                wallWidthIndicatorUI.GetComponent<TextMesh>().text = minWallWidth.ToString("0.000");
                //reset UI
                primaryUI.GetComponent<TextMesh>().text = "Practice";
                secondaryUI.GetComponent<TextMesh>().text = "Finished. Enter Shoulder Width...";
                minWallWidth = 1.5f;
                collided = false;
            }
            ++trialNum;
            float curWallWidth = transform.position.x - 7.5f;
            if (minWallWidth - curWallWidth > 0.001f) minWallWidth = curWallWidth;
        }
        
    }
}
