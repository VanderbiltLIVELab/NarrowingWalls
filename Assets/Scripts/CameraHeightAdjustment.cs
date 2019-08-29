using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeightAdjustment : MonoBehaviour {

    public Transform camera;
    public Transform textUI;

    // Use this for initialization
    void Start()
    {
        updateTextUI();
    }

    private void updateTextUI()
    {
        textUI.GetComponent<TextMesh>().text = camera.position.y.ToString("0.00");
    }

    public void increaseCameraHeight()
    {
        float increasedHeight = camera.position.y + 0.01f;
        camera.position = new Vector3(camera.position.x, increasedHeight, camera.position.z);
        updateTextUI();
    }

    public void decreaseCameraHeight()
    {
        float decreasedHeight = camera.position.y - 0.01f;
        camera.position = new Vector3(camera.position.x, decreasedHeight, camera.position.z);
        updateTextUI();
    }

    public void reset()
    {
        camera.position = new Vector3(camera.position.x, 1.6f, camera.position.z);
        updateTextUI();
    }
}
