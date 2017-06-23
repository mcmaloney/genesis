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

        public void Start()
        {
            transform.position = startPosition;
        }
    }
}

