using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.UI
{
    public class SearchDestination : MonoBehaviour
    {
        public string destinationName;

        public void UserRayHit()
        {
            // tell the search map to geocode this location's name
            SearchMap parentMap = transform.parent.gameObject.GetComponent<SearchMap>();
            parentMap.Geocode(destinationName);
        }
    }
}

