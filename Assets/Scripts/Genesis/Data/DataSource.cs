using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.Data
{
    public class DataSource
    {
        public TextAsset sourceData;
        public GeoJSON.FeatureCollection parsedData;

        public DataSource(TextAsset source)
        {
            sourceData = source;
            parsedData = ParseGeoJSON(sourceData);
        }

        private GeoJSON.FeatureCollection ParseGeoJSON(TextAsset data)
        {
            return GeoJSON.GeoJSONObject.ParseAsCollection(data.text);
        }
    }
}

