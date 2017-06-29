using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.UI
{
    public class SearchDestination : MonoBehaviour
    {
        public string destinationName;

        public void OnMouseOver()
        {
            Debug.Log("Hovering search destination " + destinationName);
        }

        public void OnClick()
        {
            // tell the search map to geocode this location's name
            SearchMap parentMap = transform.parent.gameObject.GetComponent<SearchMap>();
            Debug.Log("Telling search map to geocode "  + destinationName);
            parentMap.Geocode(destinationName);
        }
    }
}

