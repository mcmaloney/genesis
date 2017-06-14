using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Mapbox.Unity.MeshGeneration.Components;

public class EyeTrackerController : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Debug.Log("Tracking headset");
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Quaternion headsetRotation = InputTracking.GetLocalRotation(VRNode.Head);
        Vector3 playerRotation = headsetRotation * transform.forward;

        Ray ray = new Ray(transform.position, playerRotation);
        if (Physics.Raycast(ray, out hit))
        {
            // Get the data about the map vector object you're looking at
            // Probably will ditch this in favor of a UX that doesn't involve focusing your head exactly for any period of time
            FeatureBehaviour hitObjectBehaviour = hit.transform.GetComponent<FeatureBehaviour>();
        }
	}
}
