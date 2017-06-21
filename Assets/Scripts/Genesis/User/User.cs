using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genesis.UI;

namespace Genesis.User
{
    public class User : MonoBehaviour
    {
        public GameObject genesisUIObject;
        public Hand rightHand; // Movement
        public Hand leftHand; // Tools

        public void Update()
        {
            if (OVRInput.GetDown(OVRInput.RawButton.Y))
            {
                toggleUI();
            }  
            
        }

        private void toggleUI()
        {
            if (genesisUIObject.active)
            {
                genesisUIObject.SetActive(false);
            } else
            {
                genesisUIObject.SetActive(true);
            }
        }
    }
}

