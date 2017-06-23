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
        public enum HoverState { HOVER, NONE };
        public HoverState _hoverState = HoverState.NONE;

        private GameObject hoverFocusObject;
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
                Hover(hit.transform.gameObject);
                Click(hoverFocusObject);
            }
            else
            {
                if (lineRenderer)
                {
                    lineRenderer.SetPositions(lineRendererInitPoints);
                }

                HoverEnd();
            }
        }

        public void Hover(GameObject focusObject)
        {
            if (_hoverState == HoverState.NONE)
            {
                hoverFocusObject = focusObject;
                hoverFocusObject.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
            }

            _hoverState = HoverState.HOVER;
        }

        public void HoverEnd()
        {
            if (_hoverState == HoverState.HOVER)
            {
                hoverFocusObject.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
            }

            _hoverState = HoverState.NONE;
        }

        public void Click(GameObject focusObject)
        {
            if (_hoverState == HoverState.HOVER)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.B))
                {
                    focusObject.SendMessage("OnBClick", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

    }
}
