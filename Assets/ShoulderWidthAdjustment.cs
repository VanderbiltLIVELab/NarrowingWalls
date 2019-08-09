using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderWidthAdjustment : MonoBehaviour {

    public Transform camera;
    public Transform textUI;

    //public static ShoulderWidthAdjustment instance;

	// Use this for initialization
	void Start () {
        updateTextUI();
    }

    private void updateTextUI ()
    {
        textUI.GetComponent<TextMesh>().text = camera.GetComponent<CapsuleCollider>().height.ToString("0.00");
    }
	
	public void increaseShoulderWidth ()
    {
        camera.GetComponent<CapsuleCollider>().height += 0.01f;
        updateTextUI();
    }

    public void decreaseShoulderWidth()
    {
        camera.GetComponent<CapsuleCollider>().height -= 0.01f;
        updateTextUI();
    }

    public void reset()
    {
        camera.GetComponent<CapsuleCollider>().height = 0.4f;
        updateTextUI();
    }
}
