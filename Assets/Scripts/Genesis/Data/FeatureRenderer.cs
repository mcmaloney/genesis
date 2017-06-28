using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Genesis.GeoPrimitives;

namespace Genesis.Data
{
    public class FeatureRenderer : MonoBehaviour
    {
        public GameObject geoPointPrefab;
        public GameObject geoPolygonPrefab;
        public GameObject geoLinePrefab;
        public Vector2d OriginPoint;
        public float WorldScale;

        public void RenderDataSource(DataSource source, GameObject world)
        {
            for (int i = 0; i < source.parsedData.features.Count; i++)
            {
                RenderFeature(source.parsedData.features[i], world);
            }
        }

        public void RenderFeature(GeoJSON.FeatureObject feature, GameObject world)
        {
            if (feature.geometry is GeoJSON.PointGeometryObject)
            {
                RenderPointObject(feature, world);
            }
            else if (feature.geometry is GeoJSON.PolygonGeometryObject || feature.geometry is GeoJSON.MultiPolygonGeometryObject)
            {
                RenderPolygonObject(feature, world);
            }
            else if (feature.geometry is GeoJSON.LineStringGeometryObject || feature.geometry is GeoJSON.MultiLineStringGeometryObject)
            {
                RenderLineObject(feature, world);
            }
        }

        public void RenderPointObject(GeoJSON.FeatureObject feature, GameObject world)
        {
            Vector2d pointCoordinates = new Vector2d(feature.geometry.AllPositions()[0].latitude, feature.geometry.AllPositions()[0].longitude);
            GameObject pointObject = Instantiate(geoPointPrefab, new Vector3(0f, -0f, 0f), Quaternion.identity);
            pointObject.name = "GeoPoint: (" + pointCoordinates.x + ", " + pointCoordinates.y + ")";
            GeoPoint point = pointObject.GetComponent<GeoPoint>();
            point.Draw(pointCoordinates, world);
        }

        public void RenderPolygonObject(GeoJSON.FeatureObject feature, GameObject world)
        {
            Vector2d[] latLonVertices = new Vector2d[feature.geometry.AllPositions().Count];

            for (int i = 0; i < feature.geometry.AllPositions().Count; i++)
            {
                latLonVertices[i] = new Vector2d(feature.geometry.AllPositions()[i].latitude, feature.geometry.AllPositions()[i].longitude);
                Debug.Log(latLonVertices[i]);
            }

            GameObject polygonObject = Instantiate(geoPolygonPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            polygonObject.name = "Polygon: (" + latLonVertices[0] + ")";
            GeoPolygon polygon = polygonObject.GetComponent<GeoPolygon>();
            polygon.Draw(latLonVertices, world);
        }

        public void RenderLineObject(GeoJSON.FeatureObject feature, GameObject world)
        {
            Vector2d[] latLonNodes = new Vector2d[feature.geometry.AllPositions().Count];

            for (int i = 0; i < feature.geometry.AllPositions().Count; i++)
            {
                latLonNodes[i] = new Vector2d(feature.geometry.AllPositions()[i].latitude, feature.geometry.AllPositions()[i].longitude);
            }

            GameObject lineObject = Instantiate(geoLinePrefab, new Vector3(0f, -0f, 0f), Quaternion.identity);
            lineObject.name = "GeoLine: (" + latLonNodes[0] + ")";
            GeoLine line = lineObject.GetComponent<GeoLine>();
            line.Draw(latLonNodes, world);
        }
    }
}

