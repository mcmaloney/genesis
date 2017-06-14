using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genesis.UI;

namespace Genesis.User
{
    public class Hand : MonoBehaviour
    {
        public OVRInput.Controller Controller;
        public LineRenderer lineRenderer;

        private Vector3[] lineRendererInitPoints;

        void Start()
        {
            if (lineRenderer)
            {
                lineRendererInitPoints = new Vector3[2] { transform.position, transform.position };
                lineRenderer.SetPositions(lineRendererInitPoints);
                lineRenderer.SetWidth(0.01f, 0.01f);
            }
            
        }

        void Update()
        {
            transform.localPosition = OVRInput.GetLocalControllerPosition(Controller);
            transform.localRotation = OVRInput.GetLocalControllerRotation(Controller);
            raycast();
        }

        private void raycast() 
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (lineRenderer)
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);
                }

                if (OVRInput.GetDown(OVRInput.RawButton.B))
                {
                    hit.transform.SendMessage("UserRayHit");
                }
            }
            else
            {
                if (lineRenderer)
                {
                    lineRenderer.SetPositions(lineRendererInitPoints);
                }
            }
        }
    }
}
