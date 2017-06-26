using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genesis.UI;

namespace Genesis.User
{
    public class User : MonoBehaviour
    {
        public GameObject genesisUIObject;
        public Vector3 UIOffset;
        public Hand rightHand; // Movement
        public Hand leftHand; // Tools
        public float movementSpeed;

        private UIController genesisUIController;

        public void Start()
        {
            genesisUIController = genesisUIObject.GetComponent<UIController>();
        }

        public void Update()
        {
            if (OVRInput.GetDown(OVRInput.RawButton.Y))
            {
                toggleUI();
            }
            Move();
            
        }

        private void toggleUI()
        {
            if (!genesisUIObject.activeSelf)
            {
                genesisUIController.SetInitialState(transform.position + UIOffset);
            } else
            {
                genesisUIController.Close();
            }
        }

        private void Move()
        {
            Vector2 stickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
            transform.position += rightHand.movementTrajectory * stickInput.y * movementSpeed;
        }
    }
}

