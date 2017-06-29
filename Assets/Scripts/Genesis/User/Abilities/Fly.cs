using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Must be assigned to Hand object
namespace Genesis.User.Abilities
{
    public class Fly : MonoBehaviour
    {
        public GameObject UserObject;
        public OVRInput.Controller Controller;
        public Vector3 stickInput;
        public Quaternion controllerRotation;
        public Vector3 movementTrajectory;
        public float movementSpeed = 0.25f;

        // Update is called once per frame
        void Update()
        {
            stickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, Controller);
            controllerRotation = OVRInput.GetLocalControllerRotation(Controller);
            movementTrajectory = transform.forward;
            UserObject.transform.position += movementTrajectory * stickInput.y * movementSpeed;
        }
    }
}

