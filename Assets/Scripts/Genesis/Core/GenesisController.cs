﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Utils;
using Genesis.Generators;
using Genesis.UI;

namespace Genesis.Core
{
    public class GenesisController : MonoBehaviour
    {
        public GameObject MapBoxServer;
        public GameObject GenesisFeatureGenerator;
        public GameObject GenesisSearchMap;
        public Vector2d OriginPoint; // World coordinates of center of center tile generated by MapBoxServer
        public float WorldScale;
        public Vector2d mapOrigin; // Where Genesis should build the world
        public int zoom;
        public Vector4 range;
        public string worldObjectName; // The name of the root object from the Mapbox server

        private MapController mapboxServerController;
        private FeatureGenerator genesisFeatureGenerator;
        private SearchMap searchMapController;
        private bool hasWorld;

        [ContextMenu("Test Build World")]
        private void TestBuildWorld()
        {
            Vector2d origin = new Vector2d(-77.01586821694521, 38.885072355144786);
            Awake();
            BuildWorld(origin);
        }

        private void Awake()
        {
            searchMapController = GenesisSearchMap.GetComponent<SearchMap>();
            mapboxServerController = MapBoxServer.GetComponent<MapController>();
            hasWorld = false;
        }

        private void Update()
        {
            if (searchMapController.HasResponse)
            {
                mapOrigin = new Vector2d(searchMapController.Coordinate.y, searchMapController.Coordinate.x);

                Debug.Log("Geocoded map origin: " + mapOrigin);
                BuildWorld(mapOrigin);
                searchMapController.HasResponse = false;
            }
        }

        private void BuildWorld(Vector2d originCoorindates)
        {
            if (hasWorld)
            {
                DestroyWorld();
            }

            genesisFeatureGenerator = GenesisFeatureGenerator.GetComponent<FeatureGenerator>();
            mapboxServerController.BuildTiles(originCoorindates, zoom, range, worldObjectName);

            // One root tile to rule them all
            OriginPoint = mapboxServerController.RootTileOrigin; 
            Debug.Log("Origin: " + OriginPoint);

            // One world scale multiplier to rule them all
            WorldScale = MapController.WorldScaleFactor; 
            Debug.Log("Scale factor: " + WorldScale);
            hasWorld = true;

            // Pass off the global variables for the created world to the FeatureGenerator
            // Maybe a better way to do this?
            genesisFeatureGenerator.OriginPoint = OriginPoint;
            genesisFeatureGenerator.WorldScale = WorldScale;

            // These calls to build things should be handled by GeoJSON parsers but just testing now
            //genesisFeatureGenerator.buildGeoPolygon(OriginPoint, WorldScale);
            //genesisFeatureGenerator.buildGeoLine(OriginPoint, WorldScale);
        }

        private void DestroyWorld()
        {
            GameObject world = GameObject.Find(worldObjectName);
            Destroy(world);
            hasWorld = false;
        }
    }
}
