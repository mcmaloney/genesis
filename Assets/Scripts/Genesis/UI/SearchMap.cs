﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity;
using Mapbox.Geocoding;
using Mapbox.Utils;

namespace Genesis.UI
{
    public class SearchMap : MonoBehaviour
    {
        public event EventHandler<EventArgs> OnGeocoderResponse;
        public ForwardGeocodeResponse Response { get; private set; }
        public string geocodedLocationName;

        ForwardGeocodeResource _resource;

        Vector2d _coordinate;
        public Vector2d Coordinate
        {
            get
            {
                return _coordinate;
            }
        }

        bool _hasResponse;
        public bool HasResponse
        {
            get
            {
                return _hasResponse;
            }

            set
            {
                _hasResponse = value;
            }
        }

        // Child SearchDestination object calls parent map to Geocode
        public void Geocode(string locationName)
        {
            Debug.Log("Search map geocoding " + locationName);
            _hasResponse = false;
            _resource = new ForwardGeocodeResource("");
            _resource.Query = locationName;
            geocodedLocationName = locationName;
            MapboxAccess.Instance.Geocoder.Geocode(_resource, HandleGeocoderResponse);
        }

        void HandleGeocoderResponse(ForwardGeocodeResponse res)
        {
            _hasResponse = true;
            _coordinate = res.Features[0].Center;
            Debug.Log("Geocoded results for " + geocodedLocationName + ": " + _coordinate);
            Response = res;
            if (OnGeocoderResponse != null)
            {
                OnGeocoderResponse(this, EventArgs.Empty);
            }
        }
    }
}