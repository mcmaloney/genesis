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
        public GameObject geoPointPrefab;
        public GameObject geoPolygonPrefab;
        public GameObject geoLinePrefab;
        public Vector2d OriginPoint;
        public float WorldScale;

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

        public void drawGeoLine(Vector2d[] nodes)
        {
            GameObject lineObject = Instantiate(geoLinePrefab, new Vector3(0f, -100f, 0f), Quaternion.identity);
            lineObject.name = "GeoLine: (" + nodes[0] + ")";
            GeoLine line = lineObject.GetComponent<GeoLine>();
            line.Draw(nodes, OriginPoint, WorldScale);
        }
    }
}
