using System;
using UnityEngine;
using Mapbox.Unity;
using Mapbox.Geocoding;
using Mapbox.Utils;

namespace Genesis.UI
{
    public class SearchMapObject : MonoBehaviour
    {
        public string locationName;
        public event EventHandler<EventArgs> OnGeocoderResponse;
        public ForwardGeocodeResponse Response { get; private set; }

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

        public void Geocode()
        {
            _hasResponse = false;
            _resource = new ForwardGeocodeResource("");
            _resource.Query = locationName;
            MapboxAccess.Instance.Geocoder.Geocode(_resource, HandleGeocoderResponse);
        }

        void HandleGeocoderResponse(ForwardGeocodeResponse res)
        {
            _hasResponse = true;
            _coordinate = res.Features[0].Center;
            Response = res;
            if (OnGeocoderResponse != null)
            {
                OnGeocoderResponse(this, EventArgs.Empty);
            }
        }
    }
}

