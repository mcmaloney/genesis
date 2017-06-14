using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;

namespace Genesis.GeoPrimitives
{
    public class GeoPoint : MonoBehaviour
    {
        public TextAsset sampleJson;
        public JSONObject pointData;
        public Vector2d latLonPoint;
        public Vector2d xyPoint;
        public Vector2 scaledPoint;
        public GameObject markerObject;
        public bool rotate = true;

        private GameObject markerInstance;

        public void buildLatLonPointFromJSON()
        {
            // The parsing of the input JSON should be broken into a new function
            pointData = new JSONObject(sampleJson.text);
            latLonPoint = new Vector2d(pointData["geometry"][1][0].n, pointData["geometry"][1][1].n);
            Debug.Log("Lat/Lon point: " + latLonPoint);
        }

        // Convert lat / lon coordinates to XY meters
        public void buildXYPoint()
        {
            xyPoint = Conversions.GeoToWorldPosition(latLonPoint.y, latLonPoint.x, new Vector2d(0, 0));
            Debug.Log("XY point: " + xyPoint);
        }

        // Scale the point to the world origin and the scaling factor
        public void buildScaledPoint(Vector2d referencePoint, float scaleFactor)
        {
            scaledPoint = new Vector2((float)(xyPoint.x - referencePoint.x) * scaleFactor, (float)(xyPoint.y - referencePoint.y) * scaleFactor);
            Debug.Log("Scaled point: " + scaledPoint);
        }

        // This primitive is interesting because it is the only one of the three that needs to call Instantiate() on its child (visible) object
        // This is because points will have physical objects attached to them always, whereas lines and polygons are typically procedurally drawn meshes
        public void Draw(Vector2d origin, float scale)
        {
            buildLatLonPointFromJSON();
            buildXYPoint();
            buildScaledPoint(origin, scale);
            GameObject markerInstance = Instantiate(markerObject, new Vector3(scaledPoint.x, gameObject.transform.position.y, scaledPoint.y), Quaternion.identity);
            markerInstance.transform.parent = gameObject.transform;
        }

        private void Update()
        {
            if (rotate)
            {
                markerInstance.transform.Rotate(0, Time.deltaTime * 3f, 0);
            }
        }

    }
}

