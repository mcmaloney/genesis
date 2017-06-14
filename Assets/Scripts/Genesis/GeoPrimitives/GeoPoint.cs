using UnityEngine;

namespace Genesis.GeoPrimitives
{
    [System.Serializable]
    public class GeoPointProperties
    {
        public float lat;
        public float lon;
        public string entityType;
        public string dataString;
    }

    public class GeoPoint : MonoBehaviour
    {
        public GeoPointProperties metadata;
        public bool rotate;

        private void Update()
        {
            if (rotate)
            {
                transform.Rotate(0, Time.deltaTime * 3f, 0);
            }
        }

    }
}

