using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassAlert : MonoBehaviour {

    private AudioSource audioSource;
    
	void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	void OnTriggerEnter (Collider other) {
        ExperimentManager.experimentManager.collided = true;
        audioSource.Play();
	}

    void OnTriggerExit (Collider other)
    {
        audioSource.Stop();
    }
    
}
