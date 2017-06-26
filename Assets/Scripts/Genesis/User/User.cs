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
            transform.position += rightHand.movementTrajectory * rightHand.stickInput.y * movementSpeed;
        }
    }
}

