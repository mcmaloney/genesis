using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHandler : MonoBehaviour {
    public GeoJSONHandler geoJSONHandler;
    public TextAsset[] sourceFiles;
    public Dictionary<string, TextAsset> dataSources;

    public void requestAddLayer(string layerName)
    {
        geoJSONHandler.parseGeoJson(sourceFiles[0]);
    }
}
