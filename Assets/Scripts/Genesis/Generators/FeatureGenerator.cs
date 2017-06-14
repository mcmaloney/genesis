using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Genesis.GeoPrimitives;
using Genesis.Utils;

namespace Genesis.Generators
{
    public class FeatureGenerator : MonoBehaviour
    {
        public float testPointLat;
        public float testPointLon;
        public GameObject geoPointPrefab;
        public GameObject geoPolygonPrefab;
        public GameObject geoLinePrefab;

        public void buildGeoPoint(Vector2d referencePoint, float scaleFactor)
        {
            // Draw 4 points to check the polygon against
            Vector2d[] testPoints = new Vector2d[] { new Vector2d(40.747535, -73.988305), new Vector2d(40.746536, -73.985949), new Vector2d(40.74592, -73.986393), new Vector2d(40.746789, -73.988452), new Vector2d(40.747535, -73.988305) };
            for (int i = 0; i < testPoints.Length; i++)
            {
                Vector2d testPointMeters = Conversions.GeoToWorldPosition(testPoints[i].x, testPoints[i].y, new Vector2d(0, 0));
                Debug.Log("Test Point Location in Meters: (" + testPointMeters.x + "," + testPointMeters.y + ")");
                Vector3 geoPointLocation = new Vector3((float)(testPointMeters.x - referencePoint.x) * scaleFactor, 0f, (float)(testPointMeters.y - referencePoint.y) * scaleFactor);
                Debug.Log("Scaled Point Location: " + geoPointLocation);
                GameObject geoPointObject = Instantiate(geoPointPrefab, geoPointLocation, Quaternion.identity);
            }
        }

        // Draw one sample polygon
        public void buildGeoPolygon(Vector2d referencePoint, float scaleFactor)
        {
            GameObject polygonObject = Instantiate(geoPolygonPrefab, new Vector3(0f, -100f, 0f), Quaternion.identity);
            GeoPolygon polygon = polygonObject.GetComponent<GeoPolygon>();
            polygon.Draw(referencePoint, scaleFactor);
        }

        // Draw one sample line
        public void buildGeoLine(Vector2d referencePoint, float scaleFactor)
        {
            GameObject lineObject = Instantiate(geoLinePrefab, new Vector3(0f, -100f, 0f), Quaternion.identity);
            GeoLine line = lineObject.GetComponent<GeoLine>();
            line.Draw(referencePoint, scaleFactor);
        }
    }
}
