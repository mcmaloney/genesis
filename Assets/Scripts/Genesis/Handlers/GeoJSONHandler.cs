using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Genesis.Generators;

public class GeoJSONHandler : MonoBehaviour {
    public TextAsset featureCollectionAsset;
    public GeoJSON.FeatureCollection featureCollection;
    public GameObject featureGeneratorObject;

    private FeatureGenerator featureGenerator;

    [ContextMenu("Parse GeoJSON")]
    public void parseFeatureCollection()
    {
        featureCollection = GeoJSON.GeoJSONObject.ParseAsCollection(featureCollectionAsset.text);
        for (int i = 0; i < featureCollection.features.Count; i++) {
            handleFeatureObject(featureCollection.features[i]);
        }
    }

    public void handleFeatureObject(GeoJSON.FeatureObject feature)
    {
        if (feature.geometry is GeoJSON.PointGeometryObject)
        {
            generatePointObject(feature);
        } else if (feature.geometry is GeoJSON.PolygonGeometryObject || feature.geometry is GeoJSON.MultiPolygonGeometryObject)
        {
            generatePolygonObject(feature);
        }
    }

    public void generatePointObject(GeoJSON.FeatureObject feature)
    {
        featureGenerator = featureGeneratorObject.GetComponent<FeatureGenerator>();
        Vector2d pointCoordiates = new Vector2d(feature.geometry.AllPositions()[0].latitude, feature.geometry.AllPositions()[0].longitude);
        featureGenerator.drawGeoPoint(pointCoordiates);
    }

    public void generatePolygonObject(GeoJSON.FeatureObject feature)
    {
        Vector2d[] latLonVertices = new Vector2d[feature.geometry.AllPositions().Count];

        for (int i = 0; i < feature.geometry.AllPositions().Count; i++)
        {
            latLonVertices[i] = new Vector2d(feature.geometry.AllPositions()[i].latitude, feature.geometry.AllPositions()[i].longitude);
            Debug.Log(latLonVertices[i]);
        }

        featureGenerator = featureGeneratorObject.GetComponent<FeatureGenerator>();
        featureGenerator.drawGeoPolygon(latLonVertices);
    }
}
