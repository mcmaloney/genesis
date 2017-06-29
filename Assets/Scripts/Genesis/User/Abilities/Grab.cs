using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Must be assigned to Hand object
namespace Genesis.User.Abilities
{
    public class Grab : MonoBehaviour
    {
        public OVRInput.Controller Controller;
        public float grabRadius;
        public LayerMask grabMask;

        private GameObject grabbedObject;
        private bool grabbing;

        public void Update()
        {
            if (!grabbing && (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) == 1 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) == 1)) GrabObject();
            if (grabbing && (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) < 1 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Controller) < 1)) DropObject();
            if (grabbing) ScaleGrabbedObject();
        }

        public bool Grabbing()
        {
            return grabbing;
        }

        private void GrabObject()
        {
            grabbing = true;
            RaycastHit[] hits;

            hits = Physics.SphereCastAll(transform.position, grabRadius, transform.forward, 0f, grabMask);

            if (hits.Length > 0)
            {
                int closestHit = 0;

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].distance < hits[closestHit].distance) closestHit = i;
                }

                grabbedObject = hits[closestHit].transform.gameObject;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject.transform.parent = transform;
            }
        }

        private void DropObject()
        {
            grabbing = false;
            if (grabbedObject != null)
            {
                grabbedObject.transform.parent = null;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject = null;
            }
        }

        private void ScaleGrabbedObject()
        {
            if (grabbedObject != null)
            {
                float thumbstickScale = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
                Vector3 scaleIncrement = new Vector3(thumbstickScale, thumbstickScale, thumbstickScale);
                grabbedObject.transform.localScale += scaleIncrement / 100f;
            }
        }
    }
}

