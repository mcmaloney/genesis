using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genesis.UI;

namespace Genesis.User
{
    public class Hand : MonoBehaviour
    {
        public OVRInput.Controller Controller;

        void Update()
        {
            transform.localPosition = OVRInput.GetLocalControllerPosition(Controller);
            transform.localRotation = OVRInput.GetLocalControllerRotation(Controller);
        }       
    }
}
