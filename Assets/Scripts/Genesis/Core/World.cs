using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;

namespace Genesis.Core
{
    public class World : MonoBehaviour
    {
        public string worldName;
        public Vector2d originCoordinates;
        public float worldScale;
        public Vector3 startPosition;
        public float userZoomInput;

        private float lastScale;
        private Vector3 scaleIncrement = new Vector3(0.001f, 0.001f, 0.001f);

        public void Start()
        {
            transform.position = startPosition;
        }

        public void Update()
        {
            Scale();
        }

        public void Scale()
        {
            float currentScale = userZoomInput;
            if (currentScale > 0 && transform.localScale.x > 0)
            {
                if (currentScale > lastScale)
                {
                    transform.localScale += scaleIncrement;
                }
                else
                {
                    transform.localScale -= scaleIncrement;
                }
            }

            lastScale = currentScale;
        }
    }
}

