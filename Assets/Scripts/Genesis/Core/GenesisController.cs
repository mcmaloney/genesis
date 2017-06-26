﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Utils;
using Genesis.Generators;
using Genesis.User;
using Genesis.UI;

namespace Genesis.Core
{
    public class GenesisController : MonoBehaviour
    {
        public GameObject MapBoxServer;
        public GameObject GenesisFeatureGenerator;
        public GameObject GenesisPlayer;
        public GameObject GenesisUI;
        public GameObject WorldPrefab;
        public Vector2d OriginPoint; // World coordinates of center of center tile generated by MapBoxServer
        public float WorldScale;
        public Vector2d mapOrigin; // Where Genesis should build the world
        public int zoom;
        public Vector4 range;
        
        
        private MapController mapboxServerController;
        private FeatureGenerator genesisFeatureGenerator;
        private Player genesisPlayer;
        private UIController uiController;
        private GameObject currentWorld; // This needs to be switchable by the user

        [ContextMenu("Test Build World")]
        private void TestBuildWorld()
        {
            Vector2d origin = new Vector2d(40.748289, -73.988226);
            Awake();
            RequestWorld("TestWorld", origin);
        }

        private void Awake()
        {
            mapboxServerController = MapBoxServer.GetComponent<MapController>();
            genesisFeatureGenerator = GenesisFeatureGenerator.GetComponent<FeatureGenerator>();
            genesisPlayer = GenesisPlayer.GetComponent<Player>();
            uiController = GenesisUI.GetComponent<UIController>();
        }

        public void Update()
        {
            if (currentWorld)
            {
                World _currentWorld = currentWorld.GetComponent<World>();
                _currentWorld.userZoomInput = genesisPlayer.Zoom();
            }
        }

        public void RequestWorld(string worldName, Vector2d originCoordinates)
        {
            Debug.Log("Building world " + worldName + " at " + originCoordinates);
            GameObject newWorld = Instantiate(WorldPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            currentWorld = newWorld;
            World _newWorld = newWorld.GetComponent<World>();
            _newWorld.worldName = worldName;
            uiController.CreateListItem(worldName);
            BuildWorld(newWorld, originCoordinates);
        }

        public void BuildWorld(GameObject rootLayer, Vector2d originCoordinates)
        {
            mapboxServerController.BuildTiles(new Vector2d(originCoordinates.y, originCoordinates.x), zoom, range, rootLayer);
            OriginPoint = mapboxServerController.RootTileOrigin;
            WorldScale = MapController.WorldScaleFactor;
            genesisFeatureGenerator.OriginPoint = OriginPoint;
            genesisFeatureGenerator.WorldScale = WorldScale;
        }

        private void DestroyWorld(GameObject world)
        {
            Destroy(world);
        }
    }
}
