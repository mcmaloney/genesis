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
        public Vector2d OriginPoint;
        public float WorldScale;

        // Draw one sample line
        public void buildGeoLine(Vector2d referencePoint, float scaleFactor)
        {
            GameObject lineObject = Instantiate(geoLinePrefab, new Vector3(0f, -100f, 0f), Quaternion.identity);
            GeoLine line = lineObject.GetComponent<GeoLine>();
            line.Draw(referencePoint, scaleFactor);
        }

        public void drawGeoPoint(Vector2d pointCoordinates)
        {
            GameObject pointObject = Instantiate(geoPointPrefab, new Vector3(0f, -100f, 0f), Quaternion.identity);
            pointObject.name = "GeoPoint: (" + pointCoordinates.x + ", " + pointCoordinates.y + ")";
            GeoPoint point = pointObject.GetComponent<GeoPoint>();
            point.Draw(pointCoordinates, OriginPoint, WorldScale);
        }

        public void drawGeoPolygon(Vector2d[] vertices)
        {
            GameObject polygonObject = Instantiate(geoPolygonPrefab, new Vector3(0f, -100f, 0f), Quaternion.identity);
            polygonObject.name = "Polygon: (" + vertices[0] + ")";
            GeoPolygon polygon = polygonObject.GetComponent<GeoPolygon>();
            polygon.Draw(vertices, OriginPoint, WorldScale);

        }
    }
}
