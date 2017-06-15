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
            GameObject pointObject = Instantiate(geoPointPrefab, new Vector3(0f, -100, 0f), Quaternion.identity);
            GeoPoint point = pointObject.GetComponent<GeoPoint>();
            point.Draw(referencePoint, scaleFactor);
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
