using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genesis.Core;

namespace Genesis.UI
{
    public class UIController : MonoBehaviour
    {
        public GameObject Genesis;
        public GameObject WorldsMenu;
        public GameObject WorldsSearchMap;

        private GenesisController genesisController;
        private SearchMap searchMap;

        public void Start()
        {
            gameObject.SetActive(false);
            genesisController = Genesis.GetComponent<GenesisController>();
            searchMap = WorldsSearchMap.GetComponent<SearchMap>();
        }

        public void Update()
        {
            if (searchMap.HasResponse)
            {
                Debug.Log("Response from Search Map for " + searchMap.geocodedLocationName + ", " + searchMap.Coordinate);
                genesisController.BuildWorld(searchMap.geocodedLocationName, searchMap.Coordinate);
                searchMap.HasResponse = false;
            }
        }

        // TODO: make this more easily configurable without hacking a Dictionary
        public void SetInitialState(Vector3 containerPosition)
        {
            transform.position = containerPosition;
            gameObject.SetActive(true);
            WorldsMenu.SetActive(true);
            WorldsSearchMap.SetActive(false);
        }

        // Default to WorldsMenu for now, but should include menu option as a parameter here (GameObject argument)
        public void CreateListItem(string listItemText)
        {
            Debug.Log("Creating list item " + listItemText);
            Menu worldMenu = WorldsMenu.GetComponent<Menu>();
            worldMenu.addListItem(listItemText);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        [ContextMenu("Test Create List Item")]
        public void TestCreateListItem()
        {
            CreateListItem("Test World");
        }
    }
}

