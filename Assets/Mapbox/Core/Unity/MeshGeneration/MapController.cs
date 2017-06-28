namespace Mapbox.Unity.MeshGeneration
{
    using UnityEngine;
    using System.Collections.Generic;
    using Mapbox.Unity.MeshGeneration.Data;
    using Mapbox.Unity;
    using Mapbox.Platform;
    using Mapbox.Unity.Utilities;
    using Utils;
    using Genesis.Core;

    /// <summary>
    /// MapController is just an helper class imitating the game/app logic controlling the map. It creates and passes the tiles requests to MapVisualization.
    /// </summary>
    public class MapController : MonoBehaviour
    {
        public static RectD ReferenceTileRect { get; set; }
        public static float WorldScaleFactor { get; set; }
        public Vector2d RootTileOrigin { get; set; }

        public MapVisualization MapVisualization;
        public float TileSize = 100;

        [SerializeField]
        private bool _snapYToZero = true;

        [Geocode]
        public string LatLng; // Won't need this anymore because Genesis handles this
        public int Zoom;
        public Vector4 Range;

        private GameObject _root;
        private Dictionary<Vector2, UnityTile> _tiles;

        /// <summary>
        /// Pulls the root world object to origin for ease of use/view
        /// </summary>
        //public void Update()
        //{
        //    if (_snapYToZero)
        //    {
        //        var ray = new Ray(new Vector3(0, 1000, 0), Vector3.down);
        //        RaycastHit rayhit;
        //        if (Physics.Raycast(ray, out rayhit))
        //        {
        //            _root.transform.position = new Vector3(0, -rayhit.point.y, 0);
        //            _snapYToZero = false;
        //        }
        //    }
        //}

        public void BuildTiles(Vector2d coordinates, int zoom, Vector4 frame, GameObject rootObject)
        {
            MapVisualization.Initialize(MapboxAccess.Instance);
            _tiles = new Dictionary<Vector2, UnityTile>();

            var v2 = Conversions.GeoToWorldPosition(coordinates.y, coordinates.x, new Vector2d(0, 0));
            Debug.Log("User GeoToWorldPosition x: " + v2.x + " User GeoToWorldPosition y: " + v2.y);

            var tms = Conversions.MetersToTile(v2, zoom);
            Debug.Log("tms: " + tms);

            ReferenceTileRect = Conversions.TileBounds(tms, zoom);
            Vector2d rootTileOriginCoordinates = Conversions.TileIdToCenterLatitudeLongitude((int)tms.x, (int)tms.y, zoom);
            RootTileOrigin = Conversions.GeoToWorldPosition(rootTileOriginCoordinates, new Vector2d(0, 0));
            Debug.Log("Root Tile Origin in Degrees: (" + rootTileOriginCoordinates.x + "," + rootTileOriginCoordinates.y + ")");
            Debug.Log("Root Tile Origin in Meters: (" + RootTileOrigin.x + "," + RootTileOrigin.y + ")");

            WorldScaleFactor = (float)(TileSize / ReferenceTileRect.Size.x);
            Debug.Log("WorldScaleFactor: " + WorldScaleFactor);

            rootObject.transform.localScale = Vector3.one * WorldScaleFactor;

            for (int i = (int)(tms.x - frame.x); i <= (tms.x + frame.z); i++)
            {
                for (int j = (int)(tms.y - frame.y); j <= (tms.y + frame.w); j++)
                {
                    var tile = new GameObject("Tile - " + i + " | " + j).AddComponent<UnityTile>();
                    _tiles.Add(new Vector2(i, j), tile);
                    tile.Zoom = zoom;
                    tile.RelativeScale = Conversions.GetTileScaleInMeters(0, Zoom) / Conversions.GetTileScaleInMeters((float)coordinates.y, Zoom);
                    tile.TileCoordinate = new Vector2(i, j);
                    tile.Rect = Conversions.TileBounds(tile.TileCoordinate, zoom);
                    Debug.Log("Tile Bounds: (" + tile.Rect.Min + ", " + tile.Rect.Max);
                    tile.transform.position = new Vector3((float)(tile.Rect.Center.x - ReferenceTileRect.Center.x), 0, (float)(tile.Rect.Center.y - ReferenceTileRect.Center.y));
                    tile.transform.SetParent(rootObject.transform, false);
                    MapVisualization.ShowTile(tile);
                }
            }

            rootObject.GetComponent<World>().tiles = _tiles;
        }

        /// <summary>
        /// Used for loading new tiles on the existing world. Unlike Execute function, doesn't destroy the existing ones.
        /// </summary>
        /// <param name="pos">Tile coordinates of the requested tile</param>
        /// <param name="zoom">Zoom/Detail level of the requested tile</param>
        public void Request(Vector2 pos, int zoom)
        {
            if (!_tiles.ContainsKey(pos))
            {
                var tile = new GameObject("Tile - " + pos.x + " | " + pos.y).AddComponent<UnityTile>();
                _tiles.Add(pos, tile);
                tile.transform.SetParent(_root.transform, false);
                tile.Zoom = zoom;
                tile.TileCoordinate = new Vector2(pos.x, pos.y);
                tile.Rect = Conversions.TileBounds(tile.TileCoordinate, zoom);
                tile.RelativeScale = Conversions.GetTileScaleInMeters(0, Zoom) /
                    Conversions.GetTileScaleInMeters((float)Conversions.MetersToLatLon(tile.Rect.Center).x, Zoom);
                tile.transform.localPosition = new Vector3((float)(tile.Rect.Center.x - ReferenceTileRect.Center.x),
                                                           0,
                                                           (float)(tile.Rect.Center.y - ReferenceTileRect.Center.y));
                MapVisualization.ShowTile(tile);
            }
        }
    }
}