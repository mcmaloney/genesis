﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Utils;
using Genesis.User;
using Genesis.UI;

namespace Genesis.Core
{
    public class GenesisController : MonoBehaviour
    {
        public GameObject GenesisPlayer;
        public GameObject GenesisUI;
        public GameObject WorldPrefab;
        public Vector2d OriginPoint; // World coordinates of center of center tile generated by MapBoxServer
        public float WorldScale;
        public Vector2d mapOrigin; // Where Genesis should build the world
        public int zoom;
        public Vector4 range;
        
        
        private Player _GenesisPlayer;
        private UIController _UIController;
        private GameObject currentWorld; // This needs to be switchable by the user

        [ContextMenu("Test Build World")]
        private void TestBuildWorld()
        {
            Vector2d origin = new Vector2d(40.748289, -73.988226);
            Awake();
            BuildWorld("TestWorld", origin);
        }

        private void Awake()
        {
            _GenesisPlayer = GenesisPlayer.GetComponent<Player>();
            _UIController = GenesisUI.GetComponent<UIController>();
        }

        public void BuildWorld(string worldName, Vector2d originCoordinates)
        {
            Debug.Log("Building world " + worldName + " at " + originCoordinates);
            GameObject newWorld = Instantiate(WorldPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            currentWorld = newWorld;
            World _newWorld = newWorld.GetComponent<World>();
            _newWorld.Build(new Vector2d(originCoordinates.y, originCoordinates.x), zoom, range, worldName);
            _UIController.CreateListItem(worldName);
        }

        private void DestroyWorld(GameObject world)
        {
            Destroy(world);
        }
    }
}
